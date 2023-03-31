using FluentValidation;

namespace PhoneBook.Application.Queries.GetReportDetailById
{
    public class GetReportDetailByIdValidator: AbstractValidator<GetReportDetailByIdRequest>
    {
        public GetReportDetailByIdValidator()
        {
            RuleFor(c => c.TraceId).NotEmpty().NotNull().WithMessage("TraceId is mandatory");
        }
    }
}
