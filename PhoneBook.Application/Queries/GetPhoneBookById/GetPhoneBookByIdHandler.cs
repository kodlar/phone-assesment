using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Queries.GetPhoneBookById
{
    public class GetPhoneBookByIdHandler : IRequestHandler<GetPhoneBookByIdRequest, GetPhoneBookByIdResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;

        public GetPhoneBookByIdHandler(IPhoneBookRepository phoneBookRepository)
        {
            this.phoneBookRepository = phoneBookRepository;
        }

        public async Task<GetPhoneBookByIdResponse> Handle(GetPhoneBookByIdRequest request, CancellationToken cancellationToken)
        {
            var response = new GetPhoneBookByIdResponse();
            var dbResult = await this.phoneBookRepository.GetPhoneBookItemByIdAsync(request.Id);
            if (dbResult != null)
            {
                response.FirstName = dbResult.FirstName;
                response.LastName = dbResult.LastName;
                response.Id = dbResult.Id;
                response.Company = dbResult.Company;
                response.Contact = dbResult.Contact != null ?  new Domain.Dto.ContactInfoDto()
                {
                    Address = dbResult.Contact.Address,
                    City = dbResult.Contact.City,
                    Country = dbResult.Contact.Country,
                    Email = phoneBookRepository.EmailList(dbResult.Contact?.Email),
                    PhoneNumber = phoneBookRepository.PhoneList(dbResult.Contact?.PhoneNumber),
                } : null;
            }
            return response;
        }
    }
}
