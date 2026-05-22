using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AnkiBridge.Application.Abstractions.Services;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Infrastructure.Services.FileStorage;

public class AzureBlobStorageService : IFileStorageService
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient)
    {
        _container = blobServiceClient.GetBlobContainerClient("uploads");
        _container.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<Result<string>> UploadAsync(
        Stream fileStream,
        string blobName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _container.GetBlobClient(blobName);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                }
            };

            await blobClient.UploadAsync(fileStream, options, cancellationToken);

            return Result<string>.Success(blobClient.Uri.ToString());
        }
        catch (RequestFailedException ex)
        {
            return Result<string>.Failure($"Azure error: {ex.Message}");
        }
    }
}