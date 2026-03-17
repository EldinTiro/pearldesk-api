using ErrorOr;
using MediatR;
using DentFlow.Tenants.Application.Interfaces;

namespace DentFlow.Tenants.Application.Queries;

public record ListTenantsQuery(
    string? SearchTerm,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20) : IRequest<ErrorOr<PagedResult<TenantResponse>>>;

public class ListTenantsQueryHandler(ITenantRepository tenantRepository)
    : IRequestHandler<ListTenantsQuery, ErrorOr<PagedResult<TenantResponse>>>
{
    public async Task<ErrorOr<PagedResult<TenantResponse>>> Handle(ListTenantsQuery query, CancellationToken cancellationToken)
    {
        var (items, total) = await tenantRepository.ListAsync(
            query.SearchTerm, query.IsActive, query.Page, query.PageSize, cancellationToken);

        var responses = items.Select(TenantResponse.FromEntity).ToList();
        return new PagedResult<TenantResponse>(responses, total, query.Page, query.PageSize);
    }
}
