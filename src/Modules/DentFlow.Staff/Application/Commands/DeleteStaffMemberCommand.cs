using ErrorOr;
using MediatR;

namespace DentFlow.Staff.Application.Commands;

public record DeleteStaffMemberCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;

