using ErrorOr;
using MediatR;

namespace DentFlow.Patients.Application.Commands;

public record DeletePatientCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;

