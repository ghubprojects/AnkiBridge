using AnkiBridge.Application.Abstractions.Translation;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge;
using AnkiBridge.Infrastructure.Services.Translation.Google;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Infrastructure.Services.Translation;

public sealed class FallbackTranslationProvider(
    CambridgeDictionaryTranslationProvider cambridge,
    GoogleTranslationProvider google)
    : ITranslationProvider
{
    public async Task<Result<IReadOnlyList<TranslationResult>>> TranslateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var cambridgeResult = await cambridge.TranslateAsync(text, cancellationToken);
        if (cambridgeResult.IsSuccess && cambridgeResult.Value.Count > 0)
            return cambridgeResult;

        return await google.TranslateAsync(text, cancellationToken);
    }
}
