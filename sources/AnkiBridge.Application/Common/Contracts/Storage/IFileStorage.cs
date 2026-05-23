using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Common.Contracts.Storage;

public interface IFileStorage
{
    Task<Result<string>> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
}
