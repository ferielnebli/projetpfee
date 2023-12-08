
using Dyno.Platform.ReferntialData.DataModel.BalanceData;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using NHibernate.AspNetCore.Identity;
using Platform.Shared.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dyno.Platform.ReferntialData.DataModel.UserData
{
    
    public class UserEntity : IdentityUser
    {
        [NotMapped]
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        #region User Data
        public virtual string FirstName { get; set; } = string.Empty;
        public virtual string LastName { get; set; } = string.Empty;
        public virtual DateTime DateOfBirth { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual string? Picture { get; set; }
        #endregion

        #region Structure
        public virtual IList<RoleEntity>? Roles { get; set; }
       
        public virtual IList<BalanceEntity>? Balances { get; set; }
        #endregion
    }
}
