using FluentValidation;

namespace DentFlow.Appointments.Application.Commands;

public class CreateAppointmentTypeCommandValidator : AbstractValidator<CreateAppointmentTypeCommand>
{
    public CreateAppointmentTypeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.DefaultDurationMinutes).InclusiveBetween(1, 480);
        RuleFor(x => x.ColorHex)
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("ColorHex must be a valid hex colour (e.g. #3B82F6).")
            .When(x => x.ColorHex is not null);
    }
}

public class UpdateAppointmentTypeCommandValidator : AbstractValidator<UpdateAppointmentTypeCommand>
{
    public UpdateAppointmentTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.DefaultDurationMinutes).InclusiveBetween(1, 480);
        RuleFor(x => x.ColorHex)
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("ColorHex must be a valid hex colour (e.g. #3B82F6).")
            .When(x => x.ColorHex is not null);
    }
}
