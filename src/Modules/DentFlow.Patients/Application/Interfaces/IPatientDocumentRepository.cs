using DentFlow.Patients.Domain;

namespace DentFlow.Patients.Application.Interfaces;

public interface IPatientDocumentRepository
{
    Task<PatientDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PatientDocument>> ListByPatientAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task AddAsync(PatientDocument document, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(PatientDocument document, CancellationToken cancellationToken = default);
}
