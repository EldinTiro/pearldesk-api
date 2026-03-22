using FluentValidation;

namespace DentFlow.Staff.Application.Commands;

public class AddBlockedTimeCommandValidator : AbstractValidator<AddBlockedTimeCommand>
{
    public AddBlockedTimeCommandValidator()
    {
        RuleFor(x => x.StaffMemberId).NotEmpty();

        RuleFor(x => x.StartAt).NotEmpty();

        RuleFor(x => x.EndAt)
            .GreaterThan(x => x.StartAt)
            .WithMessage("End time must be after start time.");

        RuleFor(x => x.AbsenceType)
            .NotEmpty()
            .Must(v => new[] { "Vacation", "Sickness", "SpecialLeave", "ReligiousHoliday", "PublicHoliday" }.Contains(v))
            .WithMessage("Invalid absence type.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
