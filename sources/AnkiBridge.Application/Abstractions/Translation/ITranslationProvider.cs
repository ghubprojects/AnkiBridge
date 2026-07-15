using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Abstractions.Translation;

public interface ITranslationProvider
{
    Task<Result<IReadOnlyList<TranslationResult>>> TranslateAsync(
        string text,
        CancellationToken cancellationToken = default);
}

public sealed record TranslationResult(string Text, TranslationSource Source);
