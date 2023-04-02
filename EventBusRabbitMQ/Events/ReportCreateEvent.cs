namespace EventBusRabbitMQ.Events
{
    public class ReportCreateEvent : IEvent
    {        
        public string TraceId { get; set; }

    }
}
