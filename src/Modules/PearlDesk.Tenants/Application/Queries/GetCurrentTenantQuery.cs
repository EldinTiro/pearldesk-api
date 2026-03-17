using ErrorOr;
using MediatR;
using PearlDesk.Domain.Tenants;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Interfaces;

namespace PearlDesk.Tenants.Application.Queries;

public record GetCurrentTenantQuery(Guid TenantId) : IRequest<ErrorOr<TenantResponse>>;

public class GetCurrentTenantQueryHandler(ITenantRepository tenantRepository)
    : IRequestHandler<GetCurrentTenantQuery, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> Handle(GetCurrentTenantQuery query, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(query.TenantId, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;
        return TenantResponse.FromEntity(tenant);
    }
}
