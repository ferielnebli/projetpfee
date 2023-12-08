using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferentialData.DTO.UserData;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Platform.Shared.Enum;
using Platform.Shared.Result;

namespace Dyno.Platform.ReferentialData.WebApi.Controllers.UserData
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;


        public UserController(IUserService userService,ILogger<UserController> logger)
        {
            _userService = userService; 
            _logger = logger; 
        }

        [HttpGet]
        [Route("GetAllUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            try
            {
                OperationResult<List<UserDTO>> userDTOs = _userService.GetAll();
                return Ok(userDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetByUserName)}");
                OperationResult<List<UserDTO>> response = new OperationResult<List<UserDTO>>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [HttpGet]
        [Route("GetById/{id}")]

        public async  Task<IActionResult> GetById(string id)
        {
            OperationResult<UserDTO> userDTO = await _userService.GetById(id);
            return Ok(userDTO);
        }

        [HttpGet]
        [Route("GetByUserName/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByUserName(string name)
        {
            try
            {
                OperationResult<UserDTO> userDTO = await _userService.GetByUserName(name);
                if(userDTO.Result == QueryResult.IsFailed)
                {
                    return BadRequest(userDTO);
                }
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetByUserName)}");
                OperationResult<UserDTO> response = new OperationResult<UserDTO>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [HttpGet]
        [Route("GetByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                OperationResult<UserDTO> userDTO = await _userService.GetByEmail(email);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetByUserName)}");
                OperationResult<UserDTO> response = new OperationResult<UserDTO>
                {
                    Result = QueryResult.InternalServerError,
                    ExceptionMessage = "An error occurred : " + ex.InnerException?.Message
                };
                return StatusCode(500, response);
            }

        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public void Delete( string id)
        {
            _userService.Delete(id);
        }
       [HttpPost]
       [Route("AddUser")]

        public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
        {
            try
            {
                OperationResult<UserDTO> result = await _userService.Create(userDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"----Something went wrong in the {nameof(Create)}");
                return StatusCode(500, "Internal Server Error, please try later!");
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task Update([FromBody] UserDTO userDTO)
        {

             await  _userService.Update(userDTO);

            //if (result.IsCompletedSuccessfully)
            //{
            //    return Ok("Successful update");
            //}
            //else
            //{
            //    return BadRequest("Update failed");
            //}
        }
        [HttpPut]
        [Route("UpdateUserInfo")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDTO userDTO)
        {
            var result = await _userService.UpdateUserInfo(userDTO);
            return Ok(result);
        }
        [HttpPut]
        [Route("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePassword) 
        {
            var result = await _userService.UpdateUserPassword(updatePassword);
            return Ok(result);
        }


    }
}
