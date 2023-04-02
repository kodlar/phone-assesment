using MediatR;

namespace PhoneBook.Application.Queries.GetReportStatusById
{
    public class GetReportStatusByIdRequest : IRequest<GetReportStatusByIdResponse>
    {
        public string TraceId { get; set; }
    }
}
