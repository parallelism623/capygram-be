using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.Identity
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }
           
            var userRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (userRole.Equals(requirement.Role) || userRole.Equals("Role:ADMIN"))
            {
                context.Succeed(requirement);
            }
        }
        
    }
}
