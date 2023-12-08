using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.Business.Services;
using Dyno.Platform.Payment.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Platform.Shared.Enum;
using Platform.Shared.Result;
using System.Security.Claims;

namespace Dyno.Platform.Payment.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayerController : ControllerBase
    {
        private readonly ILogger<PayerController> _logger;  
        private readonly IPaymentService _paymentService;

        public PayerController(ILogger<PayerController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [Route("Payer")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Payer([FromBody] PayerDTO payerDTO)
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
                };
                payerDTO.scannerUserId = createUserId;
                OperationResult<string> response = _paymentService.Payer(payerDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Payer)}");
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
