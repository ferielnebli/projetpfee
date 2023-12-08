using AutoMapper;
using Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService;
using Dyno.Platform.ReferentialData.BusinessModel.UserRole;
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.Services.RoleDataService
{
    public class RoleClaimService : IRoleClaimService
    {
        public readonly RoleManager<RoleEntity> _roleManager;
        public readonly IMapper _mapper;
        public RoleClaimService(RoleManager<RoleEntity> roleManager, IMapper mapper) 
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public async Task<IdentityResult> AddClaimToRole(RoleClaimDTO roleClaimDTO)
        {
            RoleEntity roleEntity= await _roleManager.FindByIdAsync(roleClaimDTO.RoleId);
            Claim claim = new Claim(roleClaimDTO.ClaimValue, roleClaimDTO.ClaimType);
            var result =await  _roleManager.AddClaimAsync(roleEntity, claim);
            return result;
        }



        public async Task<IList<Claim>> GetRoleClaims(string RoleId)
        {
            RoleEntity roleEntity = await _roleManager.FindByIdAsync(RoleId);
            IList<Claim> claims =await  _roleManager.GetClaimsAsync(roleEntity);
            return claims;
        }
    }
}
