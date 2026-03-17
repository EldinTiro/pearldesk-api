using ErrorOr;
using MediatR;

namespace DentFlow.Staff.Application.Queries;

public record GetStaffMemberByIdQuery(Guid Id) : IRequest<ErrorOr<StaffMemberResponse>>;

