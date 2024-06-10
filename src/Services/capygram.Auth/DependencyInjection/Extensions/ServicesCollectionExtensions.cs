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

namespace capygram.Auth.DependencyInjection.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddConfigOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserDBSetting>(configuration.GetSection("UserDatabase"));
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IJwtServices, JwtServices>(); 
            services.AddScoped<IUserServices, UserServices>();  
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddSingleton<IEncrypter, Encrypter>();
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

                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                        {
       
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        // Custom behavior when access is forbidden
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        var result = Newtonsoft.Json.JsonConvert
                            .SerializeObject(Result<string>.CreateResult(false, new ResultDetail("403", "Forbidden"), "You are forbidden"));
                        return context.Response.WriteAsync(result);
                    },
                    OnChallenge = context =>
                    {
                        // Custom behavior when challenging the user
                        context.HandleResponse(); // Suppress the default behavior
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = Newtonsoft.Json.JsonConvert
                            .SerializeObject(Result<string>.CreateResult(false, new ResultDetail("401", "Unauthorized"), "You are not authorized"));
                        return context.Response.WriteAsync(result);
                    },
                   
                };
            });

            return services;
        }
        public static IServiceCollection AddMasstransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var masstransitOptions = new MasstransitOptions();
            configuration.GetRequiredSection("MasstransitOptions").Bind(masstransitOptions);
            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(masstransitOptions.Host, masstransitOptions.VHost, h =>
                    {
                        h.Username(masstransitOptions.UserName);
                        h.Password(masstransitOptions.Password);
                    }); 
                    bus.Message<>
                });

            });

            return services;
        }
    }
}
