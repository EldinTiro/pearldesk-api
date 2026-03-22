namespace DentFlow.Patients.Application;

public record PatientDocumentResponse(
    Guid Id,
    Guid PatientId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string Category,
    string? Notes,
    Guid UploadedByUserId,
    DateTime CreatedAt);
