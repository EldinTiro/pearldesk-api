using ErrorOr;
using MediatR;
using DentFlow.Application.Common.Interfaces;
using DentFlow.Patients.Application.Interfaces;

namespace DentFlow.Patients.Application.Queries;

public class GetPatientDocumentUrlQueryHandler(
    IPatientDocumentRepository documentRepository,
    IStorageService storageService)
    : IRequestHandler<GetPatientDocumentUrlQuery, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        GetPatientDocumentUrlQuery query,
        CancellationToken cancellationToken)
    {
        var document = await documentRepository.GetByIdAsync(query.DocumentId, cancellationToken);
        if (document is null || document.PatientId != query.PatientId)
            return Error.NotFound(description: "Document not found.");

        var url = await storageService.GetPresignedUrlAsync(
            document.StorageKey, TimeSpan.FromMinutes(15), cancellationToken);
        return url;
    }
}
