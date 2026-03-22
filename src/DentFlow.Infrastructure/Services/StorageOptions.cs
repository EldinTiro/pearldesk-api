namespace DentFlow.Infrastructure.Services;

public class StorageOptions
{
    public string ServiceUrl { get; set; } = "";
    /// <summary>
    /// Public URL used when generating presigned URLs for browser access.
    /// In Docker this should be the host-accessible URL (e.g. http://localhost:9000)
    /// while ServiceUrl is the internal Docker network URL (e.g. http://minio:9000).
    /// Falls back to ServiceUrl if not set.
    /// </summary>
    public string PublicServiceUrl { get; set; } = "";
    public string AccessKey { get; set; } = "";
    public string SecretKey { get; set; } = "";
    public string BucketName { get; set; } = "dentflow-files";
    public bool UsePathStyle { get; set; } = true;
}
