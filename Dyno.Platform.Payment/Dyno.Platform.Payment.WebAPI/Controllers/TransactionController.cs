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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService,
            ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
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
                OperationResult<List<TransactionDTO>> transaction = _transactionService.GetAll();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAll)}");
                OperationResult<List<TransactionDTO>> response = new OperationResult<List<TransactionDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [Route("GetAllPagedTransactions")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll([FromQuery] PagedParameters pagedParameters)
        {
            try
            {
                OperationResult<PagedList<TransactionDTO>> result = _transactionService.GetAll(pagedParameters);

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
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
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

                OperationResult<TransactionDTO> response = _transactionService.GetById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get)}");
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [HttpGet("GetByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetByUserId(string userId)
        {
            try
            {
                OperationResult<List<TransactionDTO>> response = _transactionService.Get(transaction => transaction.SendUserId == userId || transaction.ReceiveUserId == userId);
                if(response.ObjectValue != null && response.ObjectValue.Count > 0)
                {
                    List<TransactionDTO> transactions = response.ObjectValue.OrderBy(transactions => transactions.Date).Take(3).ToList();
                    response.ObjectValue = transactions;    
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get)}");
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [HttpGet("GetHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetHistory(string userId)
        {
            try
            {
                OperationResult<List<TransactionDTO>> response = _transactionService.Get(transaction => transaction.SendUserId == userId || transaction.ReceiveUserId == userId);
                if (response.ObjectValue != null && response.ObjectValue.Count > 0)
                {
                    List<TransactionDTO> transactions = response.ObjectValue.OrderBy(transactions => transactions.Date).ToList();
                    response.ObjectValue = transactions;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Get)}");
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
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
        public IActionResult Add([FromBody] TransactionDTO transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new OperationResult<TransactionDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }

                string? createUserId = HttpContext.User.FindFirstValue("Id");

                if (createUserId == null)
                {
                    return Unauthorized(new OperationResult<TransactionDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<TransactionDTO> response = _transactionService.Create(transaction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Add)}");
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
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
        public IActionResult Update([FromBody] TransactionDTO transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new OperationResult<TransactionDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }

                string? updateUserId = HttpContext.User.FindFirstValue("Id");

                if (updateUserId == null)
                {
                    return Unauthorized(new OperationResult<TransactionDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<TransactionDTO> response = _transactionService.Update(transaction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Update)}");
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
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
                    return BadRequest(new OperationResult<TransactionDTO>
                    {
                        Result = QueryResult.IsFailed,
                        ExceptionMessage = "Model is invalid"
                    });
                }

                string? deleteUserId = HttpContext.User.FindFirstValue("Id");

                if (deleteUserId == null)
                {
                    return Unauthorized(new OperationResult<TransactionDTO>
                    {
                        Result = QueryResult.UnAuthorized,
                        ExceptionMessage = "User unauthorized !"
                    });
                }
                OperationResult<TransactionDTO> response = _transactionService.Delete(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Delete)}");
                OperationResult<PagedList<TransactionDTO>> response = new OperationResult<PagedList<TransactionDTO>>
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
