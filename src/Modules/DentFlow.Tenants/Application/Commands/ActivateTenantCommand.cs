using ErrorOr;
using MediatR;
using DentFlow.Domain.Tenants;
using DentFlow.Tenants.Application.Interfaces;

namespace DentFlow.Tenants.Application.Commands;

public record ActivateTenantCommand(Guid Id) : IRequest<ErrorOr<Updated>>;

public class ActivateTenantCommandHandler(ITenantRepository tenantRepository)
    : IRequestHandler<ActivateTenantCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(ActivateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;

        tenant.Activate();
        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        return Result.Updated;
    }
}
