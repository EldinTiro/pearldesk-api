using ErrorOr;
using MediatR;
using DentFlow.Domain.Tenants;
using DentFlow.Tenants.Application;
using DentFlow.Tenants.Application.Interfaces;

namespace DentFlow.Tenants.Application.Commands;

public class UpdateTenantCommandHandler(ITenantRepository tenantRepository)
    : IRequestHandler<UpdateTenantCommand, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> Handle(UpdateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(command.Id, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;

        tenant.UpdateName(command.Name);
        tenant.SetLogo(command.LogoBase64);
        await tenantRepository.UpdateAsync(tenant, cancellationToken);

        return TenantResponse.FromEntity(tenant);
    }
}
