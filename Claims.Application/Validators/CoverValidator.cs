using FluentValidation;

namespace Claims.Application.Validators
{
    public class CoverValidator : AbstractValidator<Cover>
    {
        public CoverValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

            RuleFor(x => x)
                .Must(cover => cover.StartDate.AddYears(1) <= cover.EndDate);
        }
    }
}