using ErrorOr;
using MediatR;
using DentFlow.Domain.Tenants;
using DentFlow.Tenants.Application;
using DentFlow.Tenants.Application.Interfaces;

namespace DentFlow.Tenants.Application.Queries;

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
