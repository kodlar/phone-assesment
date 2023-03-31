using FluentValidation;

namespace PhoneBook.Application.Queries.GetPhoneBookById
{
    public class GetPhoneBookByIdValidator : AbstractValidator<GetPhoneBookByIdRequest>
    {
        public GetPhoneBookByIdValidator()
        {
            RuleFor(c => c.Id).NotEmpty().NotNull().WithMessage("Id mandatory");
        }
    }
}
