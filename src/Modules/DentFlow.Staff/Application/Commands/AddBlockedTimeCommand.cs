using ErrorOr;
using MediatR;

namespace DentFlow.Staff.Application.Commands;

public record AddBlockedTimeCommand(
    Guid StaffMemberId,
    DateTime StartAt,
    DateTime EndAt,
    string AbsenceType,
    string? Notes) : IRequest<ErrorOr<StaffBlockedTimeResponse>>;
