using Dyno.Platform.ReferentialData.DTO.UserData;
using Dyno.Platform.ReferntialData.DataModel.UserData;
using Microsoft.AspNetCore.Identity;
using Platform.Shared.GenericService;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.IServices.IUserDataService
{
    public interface IUserService : IGenericAsyncService<UserDTO, string >
    {
        Task<OperationResult<UserDTO>> GetByUserName(string name);
        Task<OperationResult<UserDTO>> GetByEmail(string email);
        Task<OperationResult<UserDTO>> UpdateUserInfo(UpdateUserDTO update);
        Task<OperationResult<UserDTO>> UpdateUserPassword(UpdatePasswordDTO updatePassword);
    }
}
