using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Authorization
{
    public class PermissionsRequirement : IAuthorizationRequirement
    {
        public string? Permission { get; private set; }
        public string? Role { get; private set; }

        public PermissionsRequirement(string? permission, string? role)
        {
            Permission = permission;
            Role = role;
        }
    }
}
