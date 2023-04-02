using FluentValidation;

namespace PhoneBook.Application.Commands.CreateContactInfo
{
    public class CreateContactInfoValidator:AbstractValidator<CreateContactInfoRequest>
    {
        public CreateContactInfoValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().NotNull().WithMessage("UserId mandatory");
            RuleFor(c => c.ContactInfo).NotNull().WithMessage("Contact Info mandatory");
        }
    }
}
