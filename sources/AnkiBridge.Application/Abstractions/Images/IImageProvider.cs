using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Abstractions.Images;

public interface IImageProvider
{
    Task<IReadOnlyList<ImageResult>> SearchAsync(
        string query,
        int count = 3,
        int page = 1,
        CancellationToken cancellationToken = default);
}

public sealed record ImageResult(
    string PreviewUrl,
    string FullUrl,
    ImageSource Source);
