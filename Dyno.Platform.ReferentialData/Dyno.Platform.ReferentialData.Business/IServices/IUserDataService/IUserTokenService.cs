using Dyno.Platform.ReferentialData.DTO.UserData;
using Platform.Shared.GenericService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Business.IServices.IUserDataService
{
    public interface IUserTokenService : IGenericSyncService<UserTokenDTO, string>
    {
    }
}
