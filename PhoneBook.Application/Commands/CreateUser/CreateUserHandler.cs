using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;

        public CreateUserHandler(IPhoneBookRepository phoneBookRepository)
        {
            this.phoneBookRepository = phoneBookRepository;
        }

        public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateUserResponse();
            await phoneBookRepository.CreateAsync(new Domain.Entities.PhoneBook()
            {
                Company = request.Company,
                Contact = null,
                FirstName = request.FirstName,
                IsDeleted = false,
                LastName = request.LastName
            }) ;
            response.Result = true;
            return response;
        }
    }
}
