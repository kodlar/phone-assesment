using FluentValidation;

namespace PhoneBook.Application.Queries.GetReportStatusById
{
    public class GetReportStatusByIdValidator:AbstractValidator<GetReportStatusByIdRequest>
    {
        public GetReportStatusByIdValidator()
        {
            RuleFor(c => c.TraceId).NotEmpty().NotNull().WithMessage("TraceId is mandatory");
        }
    }
}
