using EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PhoneBook.Infrastructure.Data.Impl;
using PhoneBook.Infrastructure.Data.Interfaces;
using PhoneBook.Infrastructure.Settings;
using PhoneBookWorkerService;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {

    IConfiguration configuration = hostContext.Configuration;
    services.AddHostedService<Worker>();
    services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

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


    services.AddSingleton<IPhoneBookDatabaseSettings>(sp => sp.GetRequiredService<IOptions<PhoneBookDatabaseSettings>>().Value);
    services.Configure<PhoneBookDatabaseSettings>(configuration.GetSection(nameof(PhoneBookDatabaseSettings)));


    services.AddScoped<IPhoneBookContext>(sp =>
    {
        var phoneBookDatabaseSettings = sp.GetService<IPhoneBookDatabaseSettings>();
       
        return new PhoneBookContext(phoneBookDatabaseSettings);
    });

    services.AddTransient<IPhoneBookRepository, PhoneBookRepository>();
     



    })
    .Build();

host.Run();
