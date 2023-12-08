using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder();

            string[] parts = policyName.Split(',');
            string? role = null;
            string? permission = null;

            foreach (var part in parts)
            {
                if (part.StartsWith("Role", StringComparison.OrdinalIgnoreCase))
                {
                    role = part.Substring("Role.".Length);
                }
                else if (part.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
                {
                    permission = part.Substring("Permission.".Length);
                }
            }
            policy.AddRequirements(new PermissionsRequirement(permission, role));
            return Task.FromResult(policy.Build());
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}
