using Claims.Application.Dtos;
using FluentValidation;

namespace Claims.Application.Validators
{
    public class CoverValidator : AbstractValidator<CoverDto>
    {
        public CoverValidator()
        {
            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("StartDate can't be in the past.");

            RuleFor(x => x)
                .Must(cover => cover.StartDate.AddYears(1) <= cover.EndDate)
                .WithMessage("Total insurance period cannot exceed 1 year.");
        }
    }
}