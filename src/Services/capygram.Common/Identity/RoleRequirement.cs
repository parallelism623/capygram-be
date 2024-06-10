using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.Identity
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }
        public RoleRequirement(string permission)
        {
            Role = permission;
        }
    }
}
