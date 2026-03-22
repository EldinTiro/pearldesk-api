using Microsoft.EntityFrameworkCore;
using DentFlow.Infrastructure.Persistence;
using DentFlow.Patients.Application.Interfaces;
using DentFlow.Patients.Domain;

namespace DentFlow.Infrastructure.Persistence.Repositories;

public class PatientDocumentRepository(ApplicationDbContext dbContext) : IPatientDocumentRepository
{
    public async Task<PatientDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Set<PatientDocument>().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public async Task<IReadOnlyList<PatientDocument>> ListByPatientAsync(
        Guid patientId, CancellationToken cancellationToken = default) =>
        await dbContext.Set<PatientDocument>()
            .Where(d => d.PatientId == patientId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(PatientDocument document, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<PatientDocument>().AddAsync(document, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(PatientDocument document, CancellationToken cancellationToken = default)
    {
        document.SoftDelete();
        dbContext.Set<PatientDocument>().Update(document);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
