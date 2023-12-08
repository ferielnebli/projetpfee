using Dyno.Platform.ReferentialData.DTO.UserData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.DTO.RoleData
{
    public class RoleDTO :IdentityRole
    {
        [JsonIgnore]
        public List<UserDTO>? Users { get; set; } = new List<UserDTO>();
    }
}
