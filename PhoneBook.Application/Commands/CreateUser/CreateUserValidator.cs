using FluentValidation;

namespace PhoneBook.Application.Commands.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().NotNull().WithMessage("First name mandatory!");
            RuleFor(c => c.LastName).NotEmpty().NotNull().WithMessage("Last name mandatory!");            
        }
    }
}
