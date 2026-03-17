using ErrorOr;
using MediatR;
using DentFlow.Domain.Tenants;
using DentFlow.Tenants.Application.Interfaces;

namespace DentFlow.Tenants.Application.Commands;

public record DeactivateTenantCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;

public class DeactivateTenantCommandHandler(ITenantRepository tenantRepository)
    : IRequestHandler<DeactivateTenantCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeactivateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;

        tenant.Deactivate();
        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        return Result.Deleted;
    }
}
