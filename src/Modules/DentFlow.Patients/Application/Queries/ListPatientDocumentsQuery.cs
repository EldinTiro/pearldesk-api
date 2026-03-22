using ErrorOr;
using MediatR;

namespace DentFlow.Patients.Application.Queries;

public record ListPatientDocumentsQuery(Guid PatientId) : IRequest<ErrorOr<IReadOnlyList<PatientDocumentResponse>>>;
