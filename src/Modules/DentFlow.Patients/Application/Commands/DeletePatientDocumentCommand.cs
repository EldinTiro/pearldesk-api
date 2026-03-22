using ErrorOr;
using MediatR;

namespace DentFlow.Patients.Application.Commands;

public record DeletePatientDocumentCommand(
    Guid PatientId,
    Guid DocumentId) : IRequest<ErrorOr<Deleted>>;
