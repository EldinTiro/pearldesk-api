using FluentValidation;

namespace PearlDesk.Tenants.Application.Commands;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    private static readonly System.Text.RegularExpressions.Regex SlugRegex =
        new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", System.Text.RegularExpressions.RegexOptions.Compiled);

    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MinimumLength(2).MaximumLength(63)
            .Must(s => SlugRegex.IsMatch(s))
            .WithMessage("Slug must be lowercase letters, numbers and hyphens only (e.g. 'bright-smile').");

        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.Plan)
            .NotEmpty()
            .Must(p => p is "Free" or "Pro" or "Enterprise")
            .WithMessage("Plan must be Free, Pro, or Enterprise.");

        RuleFor(x => x.OwnerEmail)
            .NotEmpty().EmailAddress().MaximumLength(256);

        RuleFor(x => x.OwnerFirstName)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.OwnerLastName)
            .NotEmpty().MaximumLength(100);
    }
}
