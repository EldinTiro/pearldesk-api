using ErrorOr;
using MediatR;

namespace DentFlow.Patients.Application.Queries;

public record GetPatientDocumentUrlQuery(
    Guid PatientId,
    Guid DocumentId) : IRequest<ErrorOr<string>>;
