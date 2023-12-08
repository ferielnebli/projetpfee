using Dyno.Platform.Payment.Business.IServices;
using Dyno.Platform.Payment.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Platform.Shared.Enum;
using Platform.Shared.Pagination;
using Platform.Shared.Result;
using System.Security.Claims;

namespace Dyno.Platform.Payment.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRCodeService _qrcodeService;
        private readonly ILogger<QRCodeController> _logger;

        public QRCodeController(IQRCodeService qrcodeService,
            ILogger<QRCodeController> logger)
        {
            _qrcodeService= qrcodeService;
            _logger = logger;
        }

        #region Get
        [Route("GetAll")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            try
            {
                OperationResult<List<QRCodeDTO>> qrcodes = _qrcodeService.GetAll();
                return Ok(qrcodes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAll)}");
                OperationResult<List<QRCodeDTO>> response = new OperationResult<List<QRCodeDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [Route("GetAllPagedQRCodes")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll([FromQuery] PagedParameters pagedParameters)
        {
            try
            {
                OperationResult<PagedList<QRCodeDTO>> result = _qrcodeService.GetAll(pagedParameters);

                if (result.ObjectValue != null)
                {
                    PaginationData metadata = new PaginationData
                    {
                        TotalCount = result.ObjectValue.TotalCount,
                        PageSize = result.ObjectValue.PageSize,
                        CurrentPage = result.ObjectValue.CurrentPage,
                        TotalPages = result.ObjectValue.TotalPages,
                        HasNext = result.ObjectValue.HasNext,
                        HasPrevious = result.ObjectValue.HasPrevious
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAll)}");
                OperationResult<PagedList<QRCodeDTO>> response = new OperationResult<PagedList<QRCodeDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [Route("Get/{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(string id)
        {
            try
            {
                string? userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return Unauthorized(new OperationResult<QRCodeDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }

                OperationResult<QRCodeDTO> response = _qrcodeService.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAll)}");
                OperationResult<PagedList<QRCodeDTO>> response = new OperationResult<PagedList<QRCodeDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }
        #endregion

        #region Create 
        [Route("Create")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] QRCodeDTO qrCode)
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
                qrCode.UserCreatorId = createUserId;
                OperationResult<QRCodeDTO> response = _qrcodeService.Create(qrCode);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Add)}");
                OperationResult<PagedList<QRCodeDTO>> response = new OperationResult<PagedList<QRCodeDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }
        #endregion

        #region Update
        [Route("Update")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] QRCodeDTO qrcode)
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

                string? updateUserId = HttpContext.User.FindFirstValue("Id");

                if (updateUserId == null)
                {
                    return Unauthorized(new OperationResult<QRCodeDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                qrcode.UserScanerId = updateUserId;
                OperationResult<QRCodeDTO> response = _qrcodeService.Update(qrcode);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Update)}");
                OperationResult<PagedList<QRCodeDTO>> response = new OperationResult<PagedList<QRCodeDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }
        #endregion

        #region Delete
        [Route("Delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(string id)
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

                string? deleteUserId = HttpContext.User.FindFirstValue("Id");

                if (deleteUserId == null)
                {
                    return Unauthorized(new OperationResult<QRCodeDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<QRCodeDTO> response = _qrcodeService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Delete)}");
                OperationResult<PagedList<QRCodeDTO>> response = new OperationResult<PagedList<QRCodeDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }
        #endregion
    }
}
