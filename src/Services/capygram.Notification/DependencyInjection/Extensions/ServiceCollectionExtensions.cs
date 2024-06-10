using capygram.Notification.DependencyInjection.Options;
using capygram.Notification.Services;
using MassTransit;
using System.Reflection;

namespace capygram.Notification.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ISendMailServices, SendMailServices>();
           
            return services;
        }
        public static IServiceCollection AddConfigurationOption(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MasstransitOptions>(config.GetRequiredSection("MasstransitOptions"));
            services.Configure<SendMailOptions>(config.GetRequiredSection("SendMailOptions"));
            return services;
        }
    }
}
