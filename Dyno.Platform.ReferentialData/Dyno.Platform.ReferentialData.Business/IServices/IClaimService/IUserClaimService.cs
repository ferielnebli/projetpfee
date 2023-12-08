using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.IServices.IUserClaimService
{
    public interface IUserClaimService 
    {
        Task<IdentityResult> Create(UserClaimDTO userClaimDTO);
       Task<IList<Claim>> GetClaims(string userId);
       //Task Delete(string userId, string claimType, string claimValue);
       
    }
}
