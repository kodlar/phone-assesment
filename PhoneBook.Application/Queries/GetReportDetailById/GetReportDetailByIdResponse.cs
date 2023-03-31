using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Enums;

namespace PhoneBook.Application.Queries.GetReportDetailById
{
    public class GetReportDetailByIdResponse
    {
        public string? Id { get; set; }
        public string TraceReportId { get; set; }
        public LocationReportItem? Report { get; set; }
        public ReportEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
