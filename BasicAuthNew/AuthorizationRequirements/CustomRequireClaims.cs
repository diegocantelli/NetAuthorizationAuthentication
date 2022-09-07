using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuth.AuthorizationRequirements
{
    public class CustomRequireClaims : IAuthorizationRequirement
    {
        public string ClaimType { get; }

        public CustomRequireClaims(string claimType)
        {
            ClaimType = claimType;
        }
    }

    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaims>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CustomRequireClaims requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

            if (hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
