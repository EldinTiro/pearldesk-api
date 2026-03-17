using ErrorOr;
using FluentValidation;
using MediatR;
using DentFlow.Domain.Tenants;
using DentFlow.Tenants.Application;
using DentFlow.Tenants.Application.Interfaces;

namespace DentFlow.Tenants.Application.Commands;

public record UpdateTenantPlanCommand(Guid Id, string Plan, DateTime? ExpiresAt) : IRequest<ErrorOr<TenantResponse>>;

public class UpdateTenantPlanCommandHandler(ITenantRepository tenantRepository)
    : IRequestHandler<UpdateTenantPlanCommand, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> Handle(UpdateTenantPlanCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;

        tenant.SetPlan(command.Plan, command.ExpiresAt);
        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        return TenantResponse.FromEntity(tenant);
    }
}

public class UpdateTenantPlanCommandValidator : AbstractValidator<UpdateTenantPlanCommand>
{
    public UpdateTenantPlanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Plan)
            .NotEmpty()
            .Must(p => p is "Free" or "Pro" or "Enterprise")
            .WithMessage("Plan must be Free, Pro, or Enterprise.");
    }
}
