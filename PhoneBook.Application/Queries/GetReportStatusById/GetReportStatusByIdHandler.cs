using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Queries.GetReportStatusById
{
    public class GetReportStatusByIdHandler : IRequestHandler<GetReportStatusByIdRequest, GetReportStatusByIdResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;

        public GetReportStatusByIdHandler(IPhoneBookRepository phoneBookRepository)
        {
            this.phoneBookRepository = phoneBookRepository;
        }

        public async Task<GetReportStatusByIdResponse> Handle(GetReportStatusByIdRequest request, CancellationToken cancellationToken)
        {
            var response = new GetReportStatusByIdResponse();

            var dbResult = await phoneBookRepository.GetPhoneBookLocationReportStatusAsync(request.TraceId);

            if (dbResult != null)
            {
                response.Status = dbResult.Status.ToString();
                response.CreatedAt = dbResult.CreatedAt;
                response.Id = dbResult.Id;
            }

            return response;
        }
    }
}
