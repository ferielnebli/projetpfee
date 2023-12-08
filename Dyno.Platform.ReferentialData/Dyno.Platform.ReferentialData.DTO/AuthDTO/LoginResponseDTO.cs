using Dyno.Platform.ReferentialData.DTO.UserData;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.AuthDTO
{
    public class LoginResponseDTO
    {
        public UserTokenDTO UserToken { get; set; } = new UserTokenDTO();

        public string? RoleName { get; set; }
    }
}
