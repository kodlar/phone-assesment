using PhoneBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Application.Queries.GetReportStatusById
{
    public class GetReportStatusByIdResponse
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReportEnum Status { get; set; }
    }
}
