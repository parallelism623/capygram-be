using Microsoft.AspNetCore.Authorization;
using System.Security;

namespace capygram.Common.Identity
{
    public class MustHaveRoleAttribute : AuthorizeAttribute
    {
        public MustHaveRoleAttribute(string role)
        {
            Policy = role;
        }
    }
}
