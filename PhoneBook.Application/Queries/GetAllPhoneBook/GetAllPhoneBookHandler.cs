using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Queries.GetAllPhoneBook
{
    public class GetAllPhoneBookHandler : IRequestHandler<GetAllPhoneBookRequest, GetAllPhoneBookResponse>
    {
        private readonly IPhoneBookRepository _phoneBookRepository;

        public GetAllPhoneBookHandler(IPhoneBookRepository phoneBookRepository)
        {
            _phoneBookRepository = phoneBookRepository;
        }

        public async Task<GetAllPhoneBookResponse> Handle(GetAllPhoneBookRequest request, CancellationToken cancellationToken)
        {
            var response = new GetAllPhoneBookResponse();
            var dbResult = await _phoneBookRepository.GetPhoneBookListAsync();
            foreach (var item in dbResult)
            {
                response.Add(new Domain.Dto.QueryDto.GetAllPhoneBookResponseDto()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Company = item.Company,
                    Contact = item.Contact != null ? new Domain.Dto.ContactInfoDto()
                    {
                        Address = item.Contact?.Address,
                        City = item.Contact?.City,
                        Country = item.Contact?.Country,
                        Email = _phoneBookRepository.EmailList(item.Contact?.Email),
                        PhoneNumber = _phoneBookRepository.PhoneList(item.Contact?.PhoneNumber),
                    } : null


                }); 
        }

            return response;
        }


}
}
