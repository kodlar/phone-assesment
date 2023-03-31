using MediatR;

namespace PhoneBook.Application.Queries.GetReportDetailById
{
    public class GetReportDetailByIdRequest : IRequest<GetReportDetailByIdResponse>
    {
        public string TraceId { get; set; }
    }
}
