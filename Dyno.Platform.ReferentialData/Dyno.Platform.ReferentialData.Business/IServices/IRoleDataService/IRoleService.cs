using Dyno.Platform.ReferentialData.Business.IServices.IUserDataService;
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using Platform.Shared.GenericService;
using Platform.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.IServices.IRoleDataService
{
    public interface IRoleService : IGenericAsyncService<RoleDTO, string>
    {
        Task<OperationResult<RoleDTO>> GetByName(string name);
    }
}
