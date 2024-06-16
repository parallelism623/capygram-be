using capygram.Common.Middlewares;
using capygram.Graph.DependencyInjection.Options;

using capygram.Graph.Repositories;
using capygram.Graph.Services;
using MassTransit;
using Neo4jClient;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace capygram.Graph.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigurationMediatR (this IServiceCollection services)
        {
            services.AddMediatR(cgf => cgf.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            return services;

        }
        public static IServiceCollection ConfigurationServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPersonServices, PersonServices>();
            services.AddSingleton<ExceptionHandlingMiddleware>();
            return services;
        }
        public static IServiceCollection ConfigurationOptions(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.Configure<MasstransitOptions>(configuration.GetRequiredSection("MasstransitOptions"));
            services.Configure<Neo4jSettings>(configuration.GetRequiredSection("Neo4jSettings"));
            return services;
        }
        public  static async Task<IServiceCollection> ConfigurationNeo4j(this IServiceCollection services
            , IConfiguration configuration)
        {
            var neo4jSettings = new Neo4jSettings();
            configuration.GetRequiredSection("Neo4jSettings").Bind(neo4jSettings);
            var client = new GraphClient(new Uri(neo4jSettings.Neo4jConnection), neo4jSettings.Neo4jUser, neo4jSettings.Neo4jPassword);


            await client.ConnectAsync();
            services.AddSingleton<IGraphClient>(client);
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
                    bus.UseMessageRetry(r => r.Immediate(5));
                });
            });
            return services;
        }
    }
}
