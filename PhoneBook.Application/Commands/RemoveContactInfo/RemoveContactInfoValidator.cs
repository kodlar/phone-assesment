using FluentValidation;

namespace PhoneBook.Application.Commands.RemoveContactInfo
{
    public class RemoveContactInfoValidator:AbstractValidator<RemoveContactInfoRequest>
    {
        public RemoveContactInfoValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().NotNull().WithMessage("UserId mandatory");
        }
    }
}
