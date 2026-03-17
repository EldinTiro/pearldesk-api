using ErrorOr;
using MediatR;

namespace DentFlow.Tenants.Application.Commands;

public record UpdateTenantCommand(Guid Id, string Name, string? LogoBase64 = null) : IRequest<ErrorOr<TenantResponse>>;
