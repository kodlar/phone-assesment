using MediatR;

namespace PhoneBook.Application.Commands.CreateUser
{
    public class CreateUserRequest : IRequest<CreateUserResponse>
    {
        public string Company { get;  set; }
        public string FirstName { get;  set; }
        public string LastName { get; set; }
    }
}
