using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferntialData.DataModel.UserData
{
    public class UserTokenEntity
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();

        #region Token
        public virtual string Token { get; set; } = string.Empty;
        public virtual string RefreshToken { get; set; } = string.Empty;
        public virtual DateTime ExpiredDate { get; set; }
        #endregion

        [ForeignKey("user_id")]
        public virtual UserEntity User { get; set; } = new UserEntity();
    }
}
