using FluentValidation;

namespace PhoneBook.Application.Commands.ReemoveUser
{
    public  class RemoveUserValidator :AbstractValidator<RemoveUserRequest>
    {
        public RemoveUserValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().NotNull().WithMessage("User Id mandatory");
        }
    }
}
