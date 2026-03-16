using ErrorOr;
using MediatR;
using PearlDesk.Domain.Tenants;
using PearlDesk.Tenants.Application.Interfaces;

namespace PearlDesk.Tenants.Application.Commands;

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
