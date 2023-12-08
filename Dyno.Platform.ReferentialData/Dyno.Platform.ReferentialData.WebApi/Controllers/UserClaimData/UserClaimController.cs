
using Dyno.Platform.ReferentialData.Business.IServices.IUserClaimService;
using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Dyno.Platform.ReferentialData.WebApi.Controllers.UserClaimData
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserClaimController : ControllerBase
    {
        public readonly IUserClaimService _userClaimService;
        public readonly ILogger<UserClaimController> _logger;   
        
        public UserClaimController(IUserClaimService userClaimService, ILogger<UserClaimController> logger) 
        {
            _userClaimService = userClaimService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUserClaims/{id}")]
        public async Task<IActionResult> GetClaims(string id) 
        {
            IList<Claim> claims= await _userClaimService.GetClaims(id);
            return Ok(claims);
        }

        [HttpPost]
        [Route("CreateUserClaim")]
         public async Task CreateUserClaim( [FromBody] UserClaimDTO userClaimDTO) 
        {
           await _userClaimService.Create(userClaimDTO);
           

        }

        //[HttpDelete]
        //[Route("DeleteUserClaim")]
        //public async Task DeleteUserClaim([FromBody] UserClaimDTO userClaimDTO)
        //{
        //    await _userClaimService.Delete(userClaimDTO);
        //}

    }
}
