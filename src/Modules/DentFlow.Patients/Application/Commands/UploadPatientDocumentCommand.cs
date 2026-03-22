using ErrorOr;
using MediatR;

namespace DentFlow.Patients.Application.Commands;

public record UploadPatientDocumentCommand(
    Guid PatientId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    Stream Content,
    string Category,
    string? Notes,
    Guid UploadedByUserId) : IRequest<ErrorOr<PatientDocumentResponse>>;
