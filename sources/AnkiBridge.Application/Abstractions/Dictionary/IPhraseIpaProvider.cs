using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Abstractions.Dictionary;

public interface IPhraseIpaProvider
{
    Task<Result<string>> ResolveAsync(
        string phrase,
        Accent accent,
        CancellationToken cancellationToken = default);
}
