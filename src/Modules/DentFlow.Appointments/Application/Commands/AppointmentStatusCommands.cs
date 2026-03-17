using ErrorOr;
using MediatR;

namespace DentFlow.Appointments.Application.Commands;

public record RescheduleAppointmentCommand(
    Guid Id,
    DateTime NewStartAt,
    DateTime NewEndAt) : IRequest<ErrorOr<AppointmentResponse>>;

public record CancelAppointmentCommand(
    Guid Id,
    string? Reason,
    Guid? CancelledByUserId) : IRequest<ErrorOr<AppointmentResponse>>;

public record UpdateAppointmentStatusCommand(
    Guid Id,
    string NewStatus) : IRequest<ErrorOr<AppointmentResponse>>;

