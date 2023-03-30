using MediatR;

namespace PhoneBook.Application.Commands.RemoveContactInfo
{
    public class RemoveContactInfoRequest : IRequest<RemoveContactInfoResponse>
    {
        public string UserId { get;  set; }
        public List<string>? PhoneIds { get; set; }
        public List<string>? EmailIds { get; set; }
    }
}
