using Dyno.Platform.Payment.DTO;
using Dyno.Platform.ReferentialData.Business.IServices.IQRCodeService;
using Dyno.Platform.ReferentialData.DTO.PaymentDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System.Security.Claims;

namespace Dyno.Platform.ReferentialData.WebApi.Controllers.QRCodeInformation
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeInformationController : ControllerBase
    {
        public readonly IQRCodeService _qrcodeService;
        public readonly ILogger<QRCodeInformationController> _logger;

        public QRCodeInformationController(IQRCodeService qrcodeService,
            ILogger<QRCodeInformationController> logger) 
        {
            _qrcodeService = qrcodeService;
            _logger = logger;
        }

        [Route("GenerateQRCode")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateQRCode(double amount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new OperationResult<QRCodeDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }

                string? createUserId = HttpContext.User.FindFirstValue("Id");

                if (createUserId == null)
                {
                    return Unauthorized(new OperationResult<QRCodeDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<QRCodeInformationDTO> response = await _qrcodeService.GenerateQRCode(amount, createUserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GenerateQRCode)}");
                OperationResult<QRCodeInformationDTO> response = new OperationResult<QRCodeInformationDTO>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }
    }
}
