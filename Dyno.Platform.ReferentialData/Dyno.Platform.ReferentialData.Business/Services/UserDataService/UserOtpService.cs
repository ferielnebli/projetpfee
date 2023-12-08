using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Platform.Shared.EnvironmentVariable;
using Platform.Shared.Mapper;
using Platform.Shared.Result;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using AutoMapper;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;

namespace Dyno.Platform.ReferentialData.Business.Services.UserDataService
{
    public class UserOtpService : IUserOtpService
    {
        private readonly IMapperSession<UserOtpEntity> _mapperSession;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserOtpService(IMapperSession<UserOtpEntity> mapperSession,
            IUserService userService,
            IMapper mapper)
        {
            _mapperSession = mapperSession;
            _userService = userService;
            _mapper = mapper;
        }    

        public OperationResult<string> GetOtpVerificationCode(string phoneNumber, OtpType otpType)
        {
            /*TwilioClient.Init(TwilioVariable.AccountSID, TwilioVariable.AuthToken);*/
            OperationResult<List<UserDTO>> users = _userService.Get(user => user.PhoneNumber == phoneNumber);
            if(users.ObjectValue == null || users.ObjectValue.Count == 0)
            {
                return new OperationResult<string>
                {
                    Result = QueryResult.UnAuthorized,
                    ExceptionMessage = "User unauthorized ! "
                };
            }
            string Code = this.GenerateOTP();
            UserOtpEntity userOtpEntity = new UserOtpEntity
            {
                Id = Guid.NewGuid(),
                PhoneNumber = phoneNumber,
                Code = Code,
                Status = Status.InProgress,
                OtpType = otpType
            };
            _mapperSession.Add(userOtpEntity);
            
            return new OperationResult<string>
            {
                Result = QueryResult.IsSucced,
                ExceptionMessage = $"Otp verification send successfully : {Code}"
            };
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            int otps=random.Next(100000,999999);
            return otps.ToString("D6");
        }

        public OperationResult<string> VerifierCode(string newCode, string phoneNumber, OtpType otpType) 
        {
            IList<UserOtpEntity> userOtpEntities = _mapperSession.GetAllByExpression(userOtp =>
            userOtp.PhoneNumber == phoneNumber &&
            userOtp.OtpType == otpType && userOtp.Status == Status.InProgress);

            if(userOtpEntities == null || userOtpEntities.Count == 0)
            {
                return new OperationResult<string>
                {
                    Result = QueryResult.IsFailed,
                    ExceptionMessage = "Code Unfound"
                };
            }
            foreach(UserOtpEntity userOtpEntity in userOtpEntities)
            {
                if(userOtpEntity.Code == newCode)
                {
                    var entityIds = userOtpEntities.Select(e => e.Id).ToList();
                    _mapperSession.UpdateColumnForEntities<UserOtpEntity>(entityIds, "Status", Status.IsDesactive);
                    return new OperationResult<string>
                    {
                        Result = QueryResult.IsSucced,
                        ExceptionMessage = "Code is verifier"
                    };
                }
            }

            return new OperationResult<string>
            {
                Result = QueryResult.IsFailed,
                ExceptionMessage = "The Code is wrong"
            };
        }
    }
}
