using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Abstractions.Services;

public interface IFileStorageService
{
    Task<Result<string>> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
}
