using AutoMapper;
using AutoMapper.Execution;
using CoreSharp.NHibernate.PostgreSQL.Types;
using Dyno.Platform.Payment.DTO;
using Dyno.Platform.ReferentialData.Business.Helper;
using Dyno.Platform.ReferentialData.Business.IServices.IAuthentification;
using Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService;
using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferentialData.DTO.AuthDTO;
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Dyno.Platform.ReferentialData.Nhibernate;
using Dyno.Platform.ReferntialData.DataModel.RoleData;
using Dyno.Platform.ReferntialData.DataModel.UserClaim;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NHibernate;
using Npgsql;
using Org.BouncyCastle.Asn1.Crmf;
using Platform.Shared.Enum;
using Platform.Shared.EnvironmentVariable;
using Platform.Shared.Mapper;
using Platform.Shared.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Dyno.Platform.ReferentialData.Business.Services.Authentification
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        public readonly RoleManager<RoleEntity> _roleManager;
        public readonly IPaymentHelper _paymentHelper;
        private readonly ISession _session;

        private readonly IMapperSession<UserEntity> _mapperSession;
        private readonly IMapperSession<UserOtpEntity> _mapperSessionOtp;        
        private readonly IMapper _mapper;
        private readonly ILogger<AuthentificationService> _logger;
        private readonly IRoleService _roleService;
        private readonly IUserTokenService _userTokenService;
        private readonly IUserService _userService;

        public AuthentificationService(SignInManager<UserEntity> signInManager, 
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,                        
            IMapperSession<UserEntity> mapperSession,             
            IMapperSession<UserOtpEntity> mapperSessionOtp,
            IMapper mapper,
            ILogger<AuthentificationService> logger,
            IPaymentHelper paymentHelper,
            IRoleService roleService,
            IUserTokenService userTokenService,
            IUserService userService,
            ISession session)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

            _mapperSession = mapperSession;
            _mapperSessionOtp = mapperSessionOtp;

            _mapper = mapper;
            _logger = logger;

            _paymentHelper = paymentHelper;
            _roleService = roleService;
            _userTokenService = userTokenService;
            _userService = userService;

            _session= session;
        }

        public async Task<OperationResult<LoginResponseDTO>> Login(LoginModelDTO loginModelDTO)
        {
            UserEntity? userEntity = await _userManager.FindByNameAsync(loginModelDTO.UserName);
            if (userEntity != null)
            {
                var passwordHasher = new PasswordHasher<UserEntity>();
                bool isCorrectPassword = passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, loginModelDTO.Password) == PasswordVerificationResult.Success ? true : false;

                if(isCorrectPassword)
                {
                    var accessToken = GenerateToken(userEntity);
                    var refreshToken = GenerateRefreshToken();

                    User user = _mapper.Map<User>(userEntity);
                    UserDTO userDto = _mapper.Map<UserDTO>(user);
                    UserTokenDTO userToken = new UserTokenDTO
                    {
                        Token= accessToken,
                        RefreshToken= refreshToken, 
                        User = userDto
                    };
                    _session.Evict(userEntity);
                    OperationResult<UserTokenDTO> responseToken = _userTokenService.Create(userToken);

                    return new OperationResult<LoginResponseDTO>
                    {
                        Result = QueryResult.IsSucced,
                        ExceptionMessage = "No error",
                        ObjectValue = new LoginResponseDTO
                        {
                            UserToken = new UserTokenDTO
                            {
                                Token = accessToken,
                                RefreshToken = refreshToken,
                                ExpiredDate = DateTime.Now.AddDays(Convert.ToDouble(JWTVariable.RefreshTokenExpired))
                            },
                            RoleName = userEntity.Roles?.FirstOrDefault()?.Name
                        }
                    };
                }

                return new OperationResult<LoginResponseDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "User unauthorize"
                };

            }
            else
            {
                return new OperationResult<LoginResponseDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "User n'existe pas"
                };
            }

        }

        public async Task<OperationResult<UserProfileDTO>> Register(RegisterModelDTO register)
        {
            RoleEntity? roleEntity = await _roleManager.FindByNameAsync(RoleName.Client.ToString());
            if (roleEntity == null)
            {

                //Insert role client
                RoleDTO roleClient = new RoleDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleName.Client.ToString(),
                    NormalizedName = RoleName.Client.ToString().ToUpper(),
                    ConcurrencyStamp= Guid.NewGuid().ToString()
                };

                OperationResult<RoleDTO> roleClientResult = await _roleService.Create(roleClient);

                if(roleClientResult.Result != QueryResult.IsSucced)
                {
                    return new OperationResult<UserProfileDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "The Role Client Unfound"
                    };
                }
                
            }
            List<UserEntity> userEntities = _mapperSession.GetAllByExpression(User => User.PhoneNumber == register.PhoneNumber &&
            User.Roles.Contains(roleEntity));
            if (userEntities.Count != 0)
            {
                /*foreach (var entity in userEntities)
                {
                    foreach(RoleEntity role in entity.Roles)
                    {
                        if(role.Name == RoleName.Client.ToString())
                        {
                            return new OperationResult<UserProfileDTO>
                            {
                                Result = QueryResult.IsFailed,
                                ExceptionMessage = "This phone number used by other client"
                            };
                        }
                    }
                    
                }*/

                return new OperationResult<UserProfileDTO>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "This phone number used by other client"
                };
            }
            User user = _mapper.Map<User>(register);
            UserEntity userEntity = _mapper.Map<UserEntity>(user);
            userEntity.Id = Guid.NewGuid().ToString();
            userEntity.Roles.Add(roleEntity);
            var result = await _userManager.CreateAsync(userEntity, register.Password);

            if(result.Succeeded)
            {
                //creation du wallet with the user Id and balance 0 
                OperationResult<WalletDTO> wallet = _paymentHelper.createWallet(userEntity.Id, register.Type);

                if(wallet.Result == QueryResult.IsSucced)
                {
                    return new OperationResult<UserProfileDTO>
                    {
                        Result = result.Succeeded ? QueryResult.IsSucced : QueryResult.IsFailed,
                        ExceptionMessage = !result.Succeeded ? result.Errors.ToList()[0].Description : null
                    };
                }
            }

            return new OperationResult<UserProfileDTO>
            {
                Result = QueryResult.IsFailed,
                ExceptionMessage = "Wallet creation failed"
            };

        }

        public async Task<OperationResult<UserProfileDTO>> GetUserProfile(string userId)
        {
            UserEntity? userEntity = await _userManager.FindByIdAsync(userId);

            if (userEntity == null)
            {
                return new OperationResult<UserProfileDTO> {
                    Result = QueryResult.UnAuthorized,
                    ExceptionMessage = "User unauthorize"
                };
            }

            //get user balance 
            OperationResult<WalletDTO>? wallet = _paymentHelper.GetBalance(userId);
            if(wallet != null && wallet.ObjectValue != null)
            {
                //get user Transaction last 3 transaction by date time
                OperationResult<List<TransactionDTO>>? transactions = _paymentHelper.GetTransactions(userId);

                if(transactions != null )
                {
                    return new OperationResult<UserProfileDTO>
                    {
                        Result = QueryResult.IsSucced,
                        ObjectValue = new UserProfileDTO
                        {
                            FirstName = userEntity.FirstName,
                            LastName = userEntity.LastName,
                            DateOfBirth= userEntity.DateOfBirth,
                            Gender= userEntity.Gender,
                            Picture= userEntity.Picture,
                            Balance = wallet.ObjectValue.Balance,
                            Transactions = transactions.ObjectValue
                        }
                    };
                }
            }

            return new OperationResult<UserProfileDTO>
            {
                Result = QueryResult.IsSucced,
                ObjectValue = new UserProfileDTO
                {
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    DateOfBirth = userEntity.DateOfBirth,
                    Gender = userEntity.Gender,
                    Picture = userEntity.Picture,
                    Balance = 0
                }
            };



        }
        private string GenerateToken(UserEntity userEntity)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTVariable.key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("userId", userEntity.Id)
            };
            

            var token = new JwtSecurityToken(
                issuer: JWTVariable.Issuer,
                audience: JWTVariable.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(JWTVariable.AccesTokenExpired)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }

        public OperationResult<string> Logout(string userId)
        {
            OperationResult<List<UserTokenDTO>> result = _userTokenService.Get(token => token.User.Id == userId);
            if(result.ObjectValue== null)
            {
                foreach(var token in result.ObjectValue)
                {
                    OperationResult<UserTokenDTO> delete = _userTokenService.Delete(token.Id);
                }
            }

            return new OperationResult<string>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = "User Logout successfully !"
            };
        }

        public async Task<OperationResult<string>> ResetPassword(ResetPasswordDTO resetPassword)
        {
            OperationResult<List<UserDTO>> ? response = _userService.Get(user => user.PhoneNumber == resetPassword.PhoneNumber);
            if(response.ObjectValue != null && response.ObjectValue.Count == 1)
            {
                User user = _mapper.Map<User>(response.ObjectValue.FirstOrDefault());
                UserEntity userEntity = _mapper.Map<UserEntity>(user);
                var token = await _userManager.GeneratePasswordResetTokenAsync(userEntity);
                var resetpassword = await _userManager.ResetPasswordAsync(userEntity, token, resetPassword.NewPassword);

                return new OperationResult<string>
                {
                    Result = QueryResult.IsSucced,
                    ExceptionMessage = "Password changed successfully !"
                };
            }

            return new OperationResult<string>
            {
                Result = QueryResult.UnAuthorized,
                ExceptionMessage = "User unauthorized!"
            };
            
        }
    }
}
