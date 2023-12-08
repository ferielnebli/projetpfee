
using Dyno.Platform.ReferentialData.DTO.RoleData;
using Dyno.Platform.ReferentialData.DTO.UserClaimData;
using NHibernate.AspNetCore.Identity;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.UserData
{
    public class UserDTO : IdentityUser
    {

        #region User Data
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? Picture { get; set; }
        #endregion

        #region Structure
        
        public IList<RoleDTO>? Roles { get; set; }
        
        #endregion

    }
}