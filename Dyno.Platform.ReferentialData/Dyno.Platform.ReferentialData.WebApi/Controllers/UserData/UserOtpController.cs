using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System.Security.Claims;
using static NHibernate.Engine.Query.CallableParser;

namespace Dyno.Platform.ReferentialData.WebApi.Controllers.UserData
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserOtpController : ControllerBase
    {
        private readonly IUserOtpService _userOtpService;
        private readonly ILogger<UserOtpController> _logger;
        private readonly IUserService _userService;
        public UserOtpController(IUserOtpService userOtpService, 
            ILogger<UserOtpController> logger,
            IUserService userService)
        {
            _userOtpService = userOtpService;
            _logger = logger;
            _userService = userService;

        }

        [HttpGet]
        [Route("SendOTP/{phoneNumber}/{otpType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendOTP(string phoneNumber, OtpType otpType)
            {
            try
            {
                OperationResult<string> result = _userOtpService.GetOtpVerificationCode(phoneNumber, otpType);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SendOTP)}");
                return StatusCode(500, "Internal server Error. Please try later");
            }
            
        }

        [HttpGet]
        [Route("VerifyOTP/{newCode}/{phoneNumber}/{otpType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult VerifyOTP(string newCode ,string phoneNumber, OtpType otpType)
        {
            try
            {
                var result = _userOtpService.VerifierCode(newCode, phoneNumber, otpType);
                return Ok(result);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(VerifyOTP)}");
                return StatusCode(500, "Internal server Error. Please try later");
            }
            
        }
    }
}
