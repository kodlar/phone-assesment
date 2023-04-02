using MediatR;
using PhoneBook.Domain.Dto;

namespace PhoneBook.Application.Commands.CreateContactInfo
{
    public class CreateContactInfoRequest : IRequest<CreateContactInfoResponse>
    {
        public string UserId { get;  set; }
        public ContactInfoDto ContactInfo { get; set; }
    }
}
