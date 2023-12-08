using Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService;
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dyno.Platform.ReferentialData.WebApi.Controllers.RoleData
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleClaimController : ControllerBase
    {
        public readonly IRoleClaimService _roleClaimService;
        public readonly ILogger<RoleClaimController> _logger;

        public RoleClaimController(ILogger<RoleClaimController> logger, IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
            _logger = logger;
        }

        //[HttpGet]
        //[Route("GetRoleClaims/{id}")]
        //public async Task<IActionResult> GetRoleClaims(string roleId) 
        //{
        //   IList<Claim> claims = await _roleClaimService.GetRoleClaims(roleId);
        //    return Ok(claims);
        //}

        [HttpPost]
        [Route("AddClaimToRole")]
        public  async Task AddClaimToRole([FromBody] RoleClaimDTO roleClaimDTO) 
        {
            await _roleClaimService.AddClaimToRole(roleClaimDTO);  

        }

        [HttpGet]
        [Route("GetRoleClaims/{roleId}")]
        public async Task<IActionResult> GetRoleClaims(string roleId) 
        {
            IList<Claim> claims = await _roleClaimService.GetRoleClaims(roleId);
            return Ok(claims);
        }


    }
}
