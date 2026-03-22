using DentFlow.Domain.Common;

namespace DentFlow.Patients.Domain;

public class PatientDocument : TenantAuditableEntity
{
    public Guid PatientId { get; private set; }
    public string FileName { get; private set; } = default!;
    public string ContentType { get; private set; } = default!;
    public long FileSizeBytes { get; private set; }
    public string StorageKey { get; private set; } = default!;
    public string Category { get; private set; } = "Other";
    public string? Notes { get; private set; }
    public Guid UploadedByUserId { get; private set; }

    private PatientDocument() { }

    public static PatientDocument Create(
        Guid patientId,
        string fileName,
        string contentType,
        long fileSizeBytes,
        string storageKey,
        string category,
        string? notes,
        Guid uploadedByUserId)
    {
        return new PatientDocument
        {
            PatientId = patientId,
            FileName = fileName,
            ContentType = contentType,
            FileSizeBytes = fileSizeBytes,
            StorageKey = storageKey,
            Category = category,
            Notes = notes,
            UploadedByUserId = uploadedByUserId
        };
    }
}
