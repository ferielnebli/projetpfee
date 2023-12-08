using AutoMapper;
using Dyno.Platform.ReferentialData.Business.IServices.IUserClaimService;
using Dyno.Platform.ReferentialData.BusinessModel.UserClaimData;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserClaim;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using FluentNHibernate.Conventions.Inspections;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Services.ClaimService
{
    public class UserClaimService : IUserClaimService
    {
        public readonly UserManager<UserEntity> _userManager;
        public readonly IMapper _mapper;
        public UserClaimService (UserManager<UserEntity> userManager, IMapper mapper) 
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IdentityResult> Create(UserClaimDTO userClaimDTO)
            
        {
            UserEntity userEntity = await _userManager.FindByIdAsync(userClaimDTO.UserId); 

            Claim userClaimEntity = new Claim(userClaimDTO.ClaimValue, userClaimDTO.ClaimType);

            var result= await _userManager.AddClaimAsync(userEntity, userClaimEntity);
            return result;
        }

        //public async Task Delete(string userId, string claimType, string claimValue)
        //{
        //    UserEntity userEntity = await _userManager.FindByIdAsync(userId);
        //    var claims = _userManager.GetClaimsAsync(userEntity);
        //    var claim = claims.F
        //    var resultat = _userManager.RemoveClaimAsync(userEntity, claim);

        //}

        public  async Task<IList<Claim>> GetClaims(string userId)
        {
            UserEntity userEntity = await _userManager.FindByIdAsync(userId);
            IList<Claim> claims = await _userManager.GetClaimsAsync(userEntity);
            return claims;   


        }

    }
}
