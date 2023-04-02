using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Commands.CreateLocationReport
{
    public class CreateLocationReportHandler : IRequestHandler<CreateLocationReportRequest, CreateLocationReportResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;
        private readonly EventBusRabbitMQProducer _eventBus;
        public CreateLocationReportHandler(IPhoneBookRepository phoneBookRepository, EventBusRabbitMQProducer eventBus)
        {
            this.phoneBookRepository = phoneBookRepository;
            _eventBus = eventBus;
        }

        public async Task<CreateLocationReportResponse> Handle(CreateLocationReportRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateLocationReportResponse();
            var traceReportId = Guid.NewGuid().ToString();
            var dbResult = await phoneBookRepository.CreateReportAsync(new Domain.Entities.PhoneBookReports()
            {
                CreatedAt = DateTime.UtcNow,
                Status = Domain.Enums.ReportEnum.Pending,
                UpdatedAt = null,
                TraceReportId = traceReportId,
                Report = null                
            });

            if(dbResult)
            {
                var eventMessage = new ReportCreateEvent();
                eventMessage.TraceId = traceReportId;
                
                _eventBus.Publish(EventBusConstants.ReportCreateQueue, eventMessage);
            }
            response.TraceId = traceReportId;
            return response;
        }
    }
}
