using AngleSharp.Dom;
using AnkiBridge.Application.Abstractions.Dictionary;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Options;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Parsing;
using Humanizer;
using Microsoft.Extensions.Options;

namespace AnkiBridge.Infrastructure.Services.Dictionary.Cambridge;

public sealed class CambridgeDictionaryEntryProvider(IOptions<CambridgeDictionaryOptions> options)
    : CambridgeDictionaryProviderBase(options), IDictionaryEntryProvider
{
    private const string Section = "english";

    private static readonly string[] EntryBlockSelectors =
    [
        ".dictionary[data-id='cald4'] .entry-body__el",
        ".dictionary[data-id='cacd'] .entry-body__el",
        ".dictionary[data-id='cald4'] .idiom-block",
        ".dictionary[data-id='cacd'] .idiom-block",
    ];

    private static readonly string[] HeadwordSelectors = [".dhw", ".hw"];
    private static readonly string[] PartOfSpeechSelectors = [".dpos", ".pos"];
    private static readonly string[] UkContainerSelectors = [".uk.dpron-i", ".uk"];
    private static readonly string[] UsContainerSelectors = [".us.dpron-i", ".us"];
    private static readonly string[] IpaSelectors = [".dpron .dipa", ".pron .ipa"];
    private static readonly string[] DefinitionBlockSelectors = [".dsense_b > .ddef_block", ".sense-body > .def-block"];
    private static readonly string[] DefinitionSelectors = [".ddef_d"];
    private static readonly string[] ExampleSelectors = [".deg", ".eg"];

    public Task<Result<IReadOnlyList<DictionaryEntryResult>>> LookupAsync(
       string headword,
       CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headword);

        var url = BuildUrl(Section, headword);

        return FetchAndParseAsync(url, ParseDocument, cancellationToken);
    }

    private Result<IReadOnlyList<DictionaryEntryResult>> ParseDocument(IDocument document)
    {
        var blocks = document.QueryAllMatch(EntryBlockSelectors);
        if (blocks.Count == 0)
            return Result.Success<IReadOnlyList<DictionaryEntryResult>>([]);

        var entries = blocks
            .Select(ParseEntryBlock)
            .OfType<DictionaryEntryResult>()
            .ToList();

        return entries;
    }

    private DictionaryEntryResult? ParseEntryBlock(IElement block)
    {
        var headword = block.QueryFirstTextMatch(HeadwordSelectors);
        if (string.IsNullOrWhiteSpace(headword))
            return null;

        return new DictionaryEntryResult(
            Headword: headword,
            PartOfSpeech: MapPartOfSpeech(block.QueryFirstTextMatch(PartOfSpeechSelectors)),
            Pronunciations: ParsePronunciations(block),
            Definitions: ParseDefinitions(block));
    }

    private static PartOfSpeech MapPartOfSpeech(string? rawLabel)
    {
        if (string.IsNullOrWhiteSpace(rawLabel))
            return PartOfSpeech.Other;

        try
        {
            return rawLabel.DehumanizeTo<PartOfSpeech>();
        }
        catch (ArgumentException)
        {
            return PartOfSpeech.Other;
        }
    }

    private List<DictionaryPronunciationResult> ParsePronunciations(IElement block)
    {
        var result = new List<DictionaryPronunciationResult>(2);
        TryAddPronunciation(block, UkContainerSelectors, IpaSelectors, Accent.British, result);
        TryAddPronunciation(block, UsContainerSelectors, IpaSelectors, Accent.American, result);
        return result;
    }

    private void TryAddPronunciation(
        IElement block,
        string[] containerSelectors,
        string[] ipaSelectors,
        Accent accent,
        List<DictionaryPronunciationResult> target)
    {
        var container = block.QueryFirstMatch(containerSelectors);
        if (container is null)
            return;

        var ipa = container.QueryFirstTextMatch(ipaSelectors);
        if (string.IsNullOrWhiteSpace(ipa))
            return;

        var audioSrc = container
            .QuerySelectorAll("source")
            .FirstOrDefault(s => s.GetAttribute("type") == "audio/mpeg")
            ?.GetAttribute("src");

        target.Add(new DictionaryPronunciationResult(
            accent,
            ipa,
            audioSrc is not null ? ToAbsoluteAudioUrl(audioSrc) : null));
    }

    private List<DictionaryDefinitionResult> ParseDefinitions(IElement block) =>
        block.QueryAllMatch(DefinitionBlockSelectors)
            .Select(ParseDefinitionBlock)
            .OfType<DictionaryDefinitionResult>()
            .ToList();

    private DictionaryDefinitionResult? ParseDefinitionBlock(IElement defBlock)
    {
        var text = defBlock.QueryFirstTextMatch(DefinitionSelectors);
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var examples = defBlock.QueryAllMatch(ExampleSelectors)
            .Select(e => e.TextContent.Trim())
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .ToList()
            .AsReadOnly();

        return new DictionaryDefinitionResult(CleanText(text), examples);
    }

    private static string ToAbsoluteAudioUrl(string src) =>
        src.StartsWith("http", StringComparison.OrdinalIgnoreCase)
            ? src
            : $"https://dictionary.cambridge.org{src}";
}
