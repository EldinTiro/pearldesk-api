using ErrorOr;
using MediatR;

namespace DentFlow.Appointments.Application.Commands;

public record CreateAppointmentTypeCommand(
    string Name,
    int DefaultDurationMinutes,
    string? Description,
    string? ColorHex,
    bool IsBookableOnline,
    decimal? DefaultFee) : IRequest<ErrorOr<AppointmentTypeResponse>>;

public record UpdateAppointmentTypeCommand(
    Guid Id,
    string Name,
    int DefaultDurationMinutes,
    string? Description,
    string? ColorHex,
    bool IsBookableOnline,
    decimal? DefaultFee) : IRequest<ErrorOr<AppointmentTypeResponse>>;

public record DeleteAppointmentTypeCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
