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
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService,
            ILogger<WalletController> logger)
        {
            _walletService = walletService;
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
                OperationResult<List<WalletDTO>> wallet = _walletService.GetAll();
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAll)}");
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [Route("GetAllPagedWallets")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll([FromQuery] PagedParameters pagedParameters)
        {
            try
            {
                OperationResult<PagedList<WalletDTO>> result = _walletService.GetAll(pagedParameters);

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
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
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
                    return Unauthorized(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }

                OperationResult<WalletDTO> response = _walletService.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get)}");
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [Route("GetByUserId/{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetByUserId(string id)
        {
            try
            {

                OperationResult<List<WalletDTO>> response = _walletService.Get(wallet => wallet.UserId == id);
                if(response.ObjectValue != null && response.ObjectValue.Count > 0)
                {
                    return Ok(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.IsSucced,
                        ObjectValue = response.ObjectValue.FirstOrDefault(),
                    });
                }
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get)}");
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
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
        public IActionResult Add([FromBody] WalletDTO wallet)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }
                OperationResult<WalletDTO> response = _walletService.Create(wallet);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Add)}");
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
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
        public IActionResult Update([FromBody] WalletDTO wallet)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }

                string? updateUserId = HttpContext.User.FindFirstValue("Id");

                if (updateUserId == null)
                {
                    return Unauthorized(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<WalletDTO> response = _walletService.Update(wallet);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Update)}");
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
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
                    return BadRequest(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }

                string? deleteUserId = HttpContext.User.FindFirstValue("Id");

                if (deleteUserId == null)
                {
                    return Unauthorized(new OperationResult<WalletDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<WalletDTO> response = _walletService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Delete)}");
                OperationResult<PagedList<WalletDTO>> response = new OperationResult<PagedList<WalletDTO>>
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
