using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Platform.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
    {
        private readonly ILogger<PermissionAuthorizationHandler> _logger;
        public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }
            var canAccessRole = context.User.Claims.Any(c => c.Type == "Role" && c.Value == requirement.Role);
            if (canAccessRole)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            var canAccessPermission = context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission) || context.User.IsInRole(DefaultRoles.SuperAdmin);
            if (canAccessPermission)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
