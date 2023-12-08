using Dyno.Platform.ReferentialData.Business.IServices.IAuthentification;
using Dyno.Platform.ReferentialData.DTO.AuthDTO;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Dyno.Platform.ReferentialData.WebApi.Controllers.Authentification
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        public readonly IAuthentificationService _authentificationService;
        public readonly ILogger<AuthentificationController> _logger;

        public AuthentificationController(ILogger<AuthentificationController> logger,
            IAuthentificationService authentificationService)
        {
            _logger = logger;
            _authentificationService = authentificationService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO modelDTO)
        {
            try
            {
                OperationResult<LoginResponseDTO> result = await _authentificationService.Login(modelDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                OperationResult<string> response = new OperationResult<string>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }


        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            string? userId = HttpContext.User.FindFirstValue("userId");
            if (userId == null)
            {
                return Unauthorized(new OperationResult<UserProfileDTO>
                {
                    Result = QueryResult.UnAuthorized,
                    ExceptionMessage = "User unauthorized !"
                });
            }
            OperationResult<string> result =  _authentificationService.Logout(userId);
            return Ok(result);

        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO modelDTO) 
        {
            OperationResult<UserProfileDTO> result= await _authentificationService.Register(modelDTO);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            string? userId = HttpContext.User.FindFirstValue("userId");
            if (userId == null)
            {
                return Unauthorized(new OperationResult<UserProfileDTO>
                {
                    Result = QueryResult.UnAuthorized,
                    ExceptionMessage = "User unauthorized !"
                });
            }
            OperationResult<UserProfileDTO> result = await _authentificationService.GetUserProfile(userId);
            return Ok(result);

        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ForgetPassword(ResetPasswordDTO resetPassword)
        {
            try
            {
                OperationResult<string> result = await _authentificationService.ResetPassword(resetPassword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ForgetPassword)}");
                OperationResult<string> response = new OperationResult<string>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }       
        }
    }
}
