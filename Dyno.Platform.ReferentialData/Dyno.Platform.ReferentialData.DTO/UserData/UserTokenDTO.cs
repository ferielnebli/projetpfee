using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.UserData
{
    public class UserTokenDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        #region Token
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiredDate { get; set; }
        #endregion

        public UserDTO User { get; set; } = new UserDTO();
    }
}
