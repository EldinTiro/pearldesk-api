using ErrorOr;
using MediatR;
using DentFlow.Application.Common.Interfaces;
using DentFlow.Patients.Application.Interfaces;

namespace DentFlow.Patients.Application.Commands;

public class DeletePatientDocumentCommandHandler(
    IPatientDocumentRepository documentRepository,
    IStorageService storageService)
    : IRequestHandler<DeletePatientDocumentCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeletePatientDocumentCommand command,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(command.DocumentId, cancellationToken);
        if (document is null || document.PatientId != command.PatientId)
            return Error.NotFound(description: "Document not found.");

        await storageService.DeleteAsync(document.StorageKey, cancellationToken);
        await documentRepository.SoftDeleteAsync(document, cancellationToken);

        return Result.Deleted;
    }
}
