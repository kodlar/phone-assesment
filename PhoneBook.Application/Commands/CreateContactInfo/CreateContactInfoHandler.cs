using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Commands.CreateContactInfo
{
    public class CreateContactInfoHandler : IRequestHandler<CreateContactInfoRequest, CreateContactInfoResponse>
    {
        private readonly IPhoneBookRepository _phoneBookRepository;

        public CreateContactInfoHandler(IPhoneBookRepository phoneBookRepository)
        {
            _phoneBookRepository = phoneBookRepository;
        }

        public async Task<CreateContactInfoResponse> Handle(CreateContactInfoRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateContactInfoResponse();
            var phoneBookItem = await _phoneBookRepository.GetPhoneBookItemByIdAsync(request.UserId);
            
            if(phoneBookItem == null)
            {
                if(phoneBookItem.Contact == null)
                {

                    phoneBookItem.Contact = new Domain.Entities.ContactInfo()
                    {
                        Address = request.ContactInfo.Address,
                        City = request.ContactInfo.City,
                        Country = request.ContactInfo.Country,                        
                    };

                    if(request.ContactInfo.Email.Any())
                    {
                        foreach (var item in request.ContactInfo.Email)
                        {
                            phoneBookItem.Contact.Email.Add(new Domain.Entities.EmailInfo()
                            {
                                IsSelected = item.IsSelected,
                                Email = item.Email,
                                Id = Guid.NewGuid().ToString(),
                                IsDeleted = false
                            });
                        }
                    }

                    if(request.ContactInfo.PhoneNumber.Any())
                    {
                        foreach (var item in request.ContactInfo.PhoneNumber)
                        {
                            phoneBookItem.Contact.PhoneNumber.Add(new Domain.Entities.PhoneInfo()
                            {
                                IsSelected = item.IsSelected,
                                PhoneNumber = item.PhoneNumber,
                                CountryCode = item.CountryCode,
                                Id = Guid.NewGuid().ToString(),
                                IsDeleted = false,
                                Type = item.Type
                            });
                        }
                    }
                   
                    response.Result = await _phoneBookRepository.UpdateAsync(phoneBookItem);
                }
            }
            return response;
        }
    }
}
