using EventBusRabbitMQ.Producer;
using EventBusRabbitMQ;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhoneBook.Application.PipelineBehaviours;
using RabbitMQ.Client;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace PhoneBook.Application.Bootstrap
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,  IConfiguration configuration)
        {

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));


            services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                Debug.WriteLine(configuration["EventBus:HostName"]);
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBus:HostName"]
                };
                if (!string.IsNullOrWhiteSpace(configuration["EventBus:UserName"]))
                {
                    factory.UserName = configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrWhiteSpace(configuration["EventBus:Password"]))
                {
                    factory.Password = configuration["EventBus:Password"];
                }
                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBus:RetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
            });

            services.AddSingleton<EventBusRabbitMQProducer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQProducer>>();
                var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBus:RetryCount"]);
                }

                return new EventBusRabbitMQProducer(connection, logger, retryCount);
            });

            return services;
        }
    }
}

