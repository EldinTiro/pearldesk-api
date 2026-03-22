using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DentFlow.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace DentFlow.Infrastructure.Services;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;
    private readonly string _publicServiceUrl;

    public S3StorageService(IOptions<StorageOptions> options)
    {
        var o = options.Value;
        var config = new AmazonS3Config
        {
            ForcePathStyle = o.UsePathStyle
        };

        if (!string.IsNullOrWhiteSpace(o.ServiceUrl))
            config.ServiceURL = o.ServiceUrl;
        else
            config.RegionEndpoint = RegionEndpoint.USEast1;

        _s3 = new AmazonS3Client(o.AccessKey, o.SecretKey, config);
        _bucket = o.BucketName;
        // Use PublicServiceUrl for presigned URLs (browser-accessible), fall back to ServiceUrl
        _publicServiceUrl = !string.IsNullOrWhiteSpace(o.PublicServiceUrl) ? o.PublicServiceUrl : o.ServiceUrl;
    }

    public async Task<string> UploadAsync(
        string key, Stream content, string contentType, CancellationToken ct = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType,
            AutoCloseStream = false
        };
        await _s3.PutObjectAsync(request, ct);
        return key;
    }

    public Task<string> GetPresignedUrlAsync(string key, TimeSpan expiry, CancellationToken ct = default)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucket,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiry),
            Verb = HttpVerb.GET
        };
        var url = _s3.GetPreSignedURL(request);

        // Rewrite internal Docker hostname to browser-accessible public URL
        if (!string.IsNullOrWhiteSpace(_publicServiceUrl))
        {
            var publicUri = new Uri(_publicServiceUrl);
            var builder = new UriBuilder(url)
            {
                Scheme = publicUri.Scheme,
                Host = publicUri.Host,
                Port = publicUri.Port
            };
            url = builder.Uri.AbsoluteUri;
        }

        return Task.FromResult(url);
    }

    public async Task DeleteAsync(string key, CancellationToken ct = default)
    {
        await _s3.DeleteObjectAsync(_bucket, key, ct);
    }
}
