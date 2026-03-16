using ErrorOr;
using MediatR;
using PearlDesk.Domain.Tenants;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Interfaces;

namespace PearlDesk.Tenants.Application.Queries;

public record GetTenantByIdQuery(Guid Id) : IRequest<ErrorOr<TenantResponse>>;

public class GetTenantByIdQueryHandler(ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantByIdQuery, ErrorOr<TenantResponse>>
{
    public async Task<ErrorOr<TenantResponse>> Handle(GetTenantByIdQuery query, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetByIdAsync(query.Id, cancellationToken);
        if (tenant is null) return TenantErrors.NotFound;
        return TenantResponse.FromEntity(tenant);
    }
}
