using ErrorOr;
using MediatR;

namespace DentFlow.Patients.Application.Queries;

public record GetPatientByIdQuery(Guid Id) : IRequest<ErrorOr<PatientResponse>>;

