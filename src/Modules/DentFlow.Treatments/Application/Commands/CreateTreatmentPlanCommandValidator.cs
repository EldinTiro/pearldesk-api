using FluentValidation;

namespace DentFlow.Treatments.Application.Commands;

public class CreateTreatmentPlanCommandValidator : AbstractValidator<CreateTreatmentPlanCommand>
{
    public CreateTreatmentPlanCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Notes).MaximumLength(2000).When(x => x.Notes is not null);
    }
}
