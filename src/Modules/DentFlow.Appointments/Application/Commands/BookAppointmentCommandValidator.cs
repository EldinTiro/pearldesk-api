using FluentValidation;

namespace DentFlow.Appointments.Application.Commands;

public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.ProviderId).NotEmpty();
        RuleFor(x => x.AppointmentTypeId).NotEmpty();
        RuleFor(x => x.StartAt).NotEmpty();
        RuleFor(x => x.EndAt)
            .NotEmpty()
            .GreaterThan(x => x.StartAt)
            .WithMessage("End time must be after start time.");
    }
}

