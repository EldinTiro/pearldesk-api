using ErrorOr;
using MediatR;
using PearlDesk.Domain.Tenants;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Interfaces;

namespace PearlDesk.Tenants.Application.Commands;

public class UpdateTenantCommandHandler(ITenantRepository tenantRepository)
    : IRequestHandler<UpdateTenantCommand, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> Handle(UpdateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;

        tenant.UpdateName(command.Name);
        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        return TenantResponse.FromEntity(tenant);
    }
}
