using MediatR;

namespace PhoneBook.Application.Commands.ReemoveUser
{
    public class RemoveUserRequest : IRequest<RemoveUserResponse>
    {
        public string UserId { get;  set; }
    }
}
