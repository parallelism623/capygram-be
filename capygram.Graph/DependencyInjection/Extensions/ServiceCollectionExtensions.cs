using capygram.Graph.DependencyInjection.Options;
using capygram.Graph.Repositories;
using MassTransit;
using Neo4j.Driver;
using System.Reflection;

namespace capygram.Graph.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigurationServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            return services;
        }
        public static IServiceCollection ConfigurationOptions(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.Configure<MasstransitOptions>(configuration.GetRequiredSection("MasstransitOptions"));
            return services;
        }
        public static IServiceCollection ConfigurationNeo4j(this IServiceCollection services
            , IConfiguration configuration)
        {
            var neo4jSettings = new Neo4jSettings();
            configuration.GetRequiredSection("Neo4jSettings").Bind(neo4jSettings);

            services.AddSingleton(GraphDatabase.Driver(neo4jSettings.Neo4jConnection, 
                AuthTokens.Basic(neo4jSettings.Neo4jUser,
                neo4jSettings.Neo4jPassword)));
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
