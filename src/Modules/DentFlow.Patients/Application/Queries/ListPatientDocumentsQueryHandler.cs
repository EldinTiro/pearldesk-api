using ErrorOr;
using MediatR;
using DentFlow.Patients.Application.Interfaces;
using DentFlow.Patients.Domain;

namespace DentFlow.Patients.Application.Queries;

public class ListPatientDocumentsQueryHandler(
    IPatientRepository patientRepository,
    IPatientDocumentRepository documentRepository)
    : IRequestHandler<ListPatientDocumentsQuery, ErrorOr<IReadOnlyList<PatientDocumentResponse>>>
{
    public async Task<ErrorOr<IReadOnlyList<PatientDocumentResponse>>> Handle(
        ListPatientDocumentsQuery query,
        CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByIdAsync(query.PatientId, cancellationToken);
        if (patient is null)
            return PatientErrors.NotFound;

        var docs = await documentRepository.ListByPatientAsync(query.PatientId, cancellationToken);
        return docs.Select(d => new PatientDocumentResponse(
            d.Id,
            d.PatientId,
            d.FileName,
            d.ContentType,
            d.FileSizeBytes,
            d.Category,
            d.Notes,
            d.UploadedByUserId,
            d.CreatedAt)).ToList();
    }
}
