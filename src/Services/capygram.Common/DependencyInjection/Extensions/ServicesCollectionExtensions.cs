using capygram.Common.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace capygram.Common.DependencyInjection.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection ConfigurationFluentValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
            return services;
        }
        public static IServiceCollection AddIdentityHandler(this IServiceCollection services)
        {   
            services.AddSingleton<IAuthorizationPolicyProvider, RolePolicyProvider>();
            services.AddTransient<IAuthorizationHandler, RoleAuthorizationHandler>();
            return services;
        }
    }
}
