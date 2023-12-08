using Dyno.Platform.ReferentialData.BusinessModel.UserData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.BusinessModel.UserRole
{
    public class Role :IdentityRole
    {
        [JsonIgnore]
        public IList<User> users { get; set; }
    }
}
