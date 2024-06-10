using capygram.Auth.DependencyInjection.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.IO;
using System.Text;
using Newtonsoft.Json;
using capygram.Auth.Services;
using capygram.Auth.Domain.Repositories;
using capygram.Auth.Repositories;
using capygram.Auth.Domain.Data;
using capygram.Auth.Domain.Services;
using capygram.Common.Shared;
using MassTransit;
using capygram.Common.Abstraction;
using capygram.Common.Exceptions;
using capygram.Common.Middlewares;
using System.Reflection;

namespace capygram.Auth.DependencyInjection.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddConfigOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserDBSetting>(configuration.GetSection("UserDatabase"));
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            services.Configure<MasstransitOptions>(configuration.GetSection("MasstransitOptions"));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IJwtServices, JwtServices>(); 
            services.AddScoped<IUserServices, UserServices>();  
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSingleton<IEncrypter, Encrypter>();

            services.AddSingleton<ExceptionHandlingMiddleware>();
            return services;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.GetSection("JwtOptions").Bind(jwtOptions);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
            }).AddJwtBearer(opt => 
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
                opt.Events = new JwtBearerEvents
                {
                    
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) 
                            )
                        {
       
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        // Custom behavior when access is forbidden
                        throw new ForbiddenException("User is forbiddened");
                    },
                    OnChallenge = context =>
                    {
                        // Custom behavior when challenging the user
                        throw new UnAuthorizedExeption("User is not authenticated");
                    },
                   
                };
            });

            return services;
        }

        public static IServiceCollection ConfigurationMasstransit(this IServiceCollection services, IConfiguration config)
        {
            var massOp = new MasstransitOptions();
            config.GetRequiredSection("MasstransitOptions").Bind(massOp);
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumers(Assembly.GetExecutingAssembly());
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(massOp.Host, massOp.VHost, host =>
                    {
                        host.Username(massOp.UserName);
                        host.Password(massOp.Password);
                    });
                    bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());
                    bus.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
