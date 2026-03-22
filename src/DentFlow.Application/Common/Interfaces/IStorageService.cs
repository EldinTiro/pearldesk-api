namespace DentFlow.Application.Common.Interfaces;

public interface IStorageService
{
    /// <summary>Upload a file stream. Returns the storage key (object path).</summary>
    Task<string> UploadAsync(string key, Stream stream, string contentType, CancellationToken ct = default);

    /// <summary>Generate a pre-signed URL valid for the given duration.</summary>
    Task<string> GetPresignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default);

    /// <summary>Permanently delete an object.</summary>
    Task DeleteAsync(string key, CancellationToken ct = default);
}
