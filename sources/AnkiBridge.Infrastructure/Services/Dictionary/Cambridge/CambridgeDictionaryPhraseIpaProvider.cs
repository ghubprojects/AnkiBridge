using AngleSharp.Dom;
using AnkiBridge.Application.Abstractions.Dictionary;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Options;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Parsing;
using AnkiBridge.Shared.Results;
using Microsoft.Extensions.Options;

namespace AnkiBridge.Infrastructure.Services.Dictionary.Cambridge;

public sealed class CambridgeDictionaryPhraseIpaProvider(IOptions<CambridgeDictionaryOptions> options)
    : CambridgeDictionaryProviderBase(options), IPhraseIpaProvider
{
    private const string Section = "english";
 
    private static readonly string[] EntryBlockSelectors =
    [
        ".dictionary[data-id='cald4'] .entry-body__el",
        ".dictionary[data-id='cacd'] .entry-body__el",
    ];
 
    private static readonly string[] UkContainerSelectors = [".uk.dpron-i", ".uk"];
    private static readonly string[] UsContainerSelectors = [".us.dpron-i", ".us"];
    private static readonly string[] IpaSelectors = [".dpron .dipa", ".pron .ipa"];
 
    public async Task<Result<string>> ResolveAsync(
        string headword,
        Accent accent,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headword);
 
        var words = SplitIntoWords(headword);
        if (words.Count == 0)
            return Result.Failure<string>("No word found in the given headword phrase.");
 
        var ipas = new List<string>(words.Count);
 
        foreach (var word in words)
        {
            var ipa = await LookupWordIpaAsync(word, accent, cancellationToken);
            if (ipa is null)
                return Result.Failure<string>($"Could not find IPA for '{word}'.");
 
            ipas.Add(ipa);
        }
 
        return Result.Success<string>(string.Join(" ", ipas));
    }
 
    private async Task<string?> LookupWordIpaAsync(
        string word,
        Accent accent,
        CancellationToken cancellationToken)
    {
        var url = BuildUrl(Section, word);
 
        var result = await FetchAndParseAsync(url, document => ParseIpa(document, accent), cancellationToken);
 
        return result.IsSuccess && !string.IsNullOrWhiteSpace(result.Value)
            ? result.Value
            : null;
    }
 
    private Result<string?> ParseIpa(IDocument document, Accent accent)
    {
        var containerSelectors = accent switch
        {
            Accent.British => UkContainerSelectors,
            Accent.American => UsContainerSelectors,
            _ => throw new ArgumentOutOfRangeException(nameof(accent), accent, "Unsupported accent."),
        };
 
        foreach (var block in document.QueryAllMatch(EntryBlockSelectors))
        {
            var container = block.QueryFirstMatch(containerSelectors);
            var ipa = container is null ? null : ExtractPreferWeakForm(container);
 
            if (!string.IsNullOrWhiteSpace(ipa))
                return Result.Success<string?>(ipa);
        }
 
        return Result.Success<string?>(null);
    }
 
    // Function words (e.g. "a", "and", "for") sometimes have more than one
    // pronunciation listed inside the same UK/US block: a default/weak one and one
    // explicitly marked "strong". Cambridge doesn't expose a documented, stable CSS
    // class for that label, so detection here is text-based: it reads whatever text
    // sits right before each IPA entry and looks for "weak"/"strong". Inspect the
    // live page's markup and adjust this if it stops matching.
    private static string? ExtractPreferWeakForm(IElement container)
    {
        var ipaElements = container.QueryAllMatch(IpaSelectors).ToList();
        if (ipaElements.Count == 0)
            return null;
 
        if (ipaElements.Count == 1)
            return CleanIpa(ipaElements[0]);
 
        var weak = ipaElements.FirstOrDefault(e =>
            PrecedingLabel(container, e).Contains("weak", StringComparison.OrdinalIgnoreCase));
        if (weak is not null)
            return CleanIpa(weak);
 
        var notStrong = ipaElements.FirstOrDefault(e =>
            !PrecedingLabel(container, e).Contains("strong", StringComparison.OrdinalIgnoreCase));
 
        return CleanIpa(notStrong ?? ipaElements[0]);
    }
 
    private static string PrecedingLabel(IElement container, IElement ipaElement)
    {
        var fullText = container.TextContent;
        var ipaText = ipaElement.TextContent;
        var index = fullText.IndexOf(ipaText, StringComparison.Ordinal);
 
        return index > 0 ? fullText[..index] : string.Empty;
    }
 
    private static string CleanIpa(IElement element) => element.TextContent.Trim();
 
    private static List<string> SplitIntoWords(string phrase) =>
        phrase
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim('.', ',', '!', '?', ';', ':', '"', '(', ')'))
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .ToList();
}