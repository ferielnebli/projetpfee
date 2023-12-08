using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService
{
    public interface IRoleClaimService
    {
        Task<IdentityResult> AddClaimToRole(RoleClaimDTO roleClaimDTO);
        Task<IList<Claim>> GetRoleClaims(string RoleId);
    }
}
