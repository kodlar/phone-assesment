using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Data.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PhoneBookWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IPhoneBookRepository _phoneBookRepository;

        public Worker(ILogger<Worker> logger, IRabbitMQPersistentConnection persistentConnection, IPhoneBookRepository phoneBookRepository)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _phoneBookRepository = phoneBookRepository;
        }

        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.QueueDeclare(queue: EventBusConstants.ReportCreateQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            //consumer.Received += ReceivedEvent;
            

            channel.BasicConsume(queue: EventBusConstants.ReportCreateQueue, autoAck: false, consumer: consumer);

            consumer.Received += async (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                var @event = JsonConvert.DeserializeObject<ReportCreateEvent>(message);

                if (ea.RoutingKey == EventBusConstants.ReportCreateQueue)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(7000);
                        Console.WriteLine($"TraceId: {@event.TraceId}");
                        //Fetch record from mongodb
                        var record = await _phoneBookRepository.GetPhoneBookReportDetailAsync(@event.TraceId);
                        //process and update mongodb
                        record.Report = GenerateGenericReport();
                        record.UpdatedAt = DateTime.UtcNow;
                        await _phoneBookRepository.UpdateAsync(record);

                        Console.WriteLine("rapor iþlemi tamamlandý");
                    });
                    

                }

                //var body = ea.Body.ToArray();
                // positively acknowledge a single delivery, the message will
                // be discarded
                channel.BasicAck(ea.DeliveryTag, true);
            };

        }

        private LocationReportItem GenerateGenericReport()
        {
            var record = new LocationReportItem();
            record.Location = RandomString(7);
            record.PhoneNumberCount = random.Next(10);
            record.PersonCount = random.Next(5);
            return record;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void ReceivedEvent(object? sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<ReportCreateEvent>(message);

            if (e.RoutingKey == EventBusConstants.ReportCreateQueue)
            {
                Console.WriteLine($"TraceId: {@event.TraceId}");

                
            }
           
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            if (!stoppingToken.IsCancellationRequested)
            {
                
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //Rabbitmq container ayaða kalkmasýný beklemek için bu süre eklendi
                //yoksa baðlantý hatasý veriyor...
                await Task.Delay(5000);
                this.Consume();
            }
        }
    }
}