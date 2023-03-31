using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Queries.GetReportDetailById
{
    public class GetReportDetailByIdHandler : IRequestHandler<GetReportDetailByIdRequest, GetReportDetailByIdResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;

        public GetReportDetailByIdHandler(IPhoneBookRepository phoneBookRepository)
        {
            this.phoneBookRepository = phoneBookRepository;
        }

        public async Task<GetReportDetailByIdResponse> Handle(GetReportDetailByIdRequest request, CancellationToken cancellationToken)
        {
            var response = new GetReportDetailByIdResponse();
            var dbResult = await phoneBookRepository.GetPhoneBookReportDetailAsync(request.TraceId);
            if(dbResult != null)
            {
                response.Id = dbResult.Id;
                response.Report = dbResult.Report;
                response.TraceReportId = dbResult.TraceReportId;
                response.CreatedAt = dbResult.CreatedAt;    
                response.UpdatedAt = dbResult.UpdatedAt;
                response.Status = dbResult.Status.ToString();                
            }
            return response;
        }
    }
}
