using AngleSharp.Dom;
using AnkiBridge.Application.Abstractions.Translation;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Options;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Parsing;
using Microsoft.Extensions.Options;

namespace AnkiBridge.Infrastructure.Services.Dictionary.Cambridge;

public sealed class CambridgeDictionaryTranslationProvider(IOptions<CambridgeDictionaryOptions> options)
    : CambridgeDictionaryProviderBase(options), ITranslationProvider
{
    private const string Section = "english-vietnamese";

    private static readonly string[] EntryBlockSelectors =
    [
        ".dictionary[data-id='cenv'] .entry-body__el",
        ".dictionary[data-id='cenv'] .idiom-block",
    ];

    private static readonly string[] DefinitionBlockSelectors = [".dsense_b > .ddef_block", ".sense-body > .def-block"];
    private static readonly string[] DefinitionSelectors = [".ddef_d"];
    private static readonly string[] TranslationSelectors = [".dtrans", ".trans"];

    public Task<Result<IReadOnlyList<TranslationResult>>> TranslateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var url = BuildUrl(Section, text);
        return FetchAndParseAsync(url, ParseDocument, cancellationToken);
    }

    private Result<IReadOnlyList<TranslationResult>> ParseDocument(IDocument document)
    {
        var entryBlocks = document.QueryAllMatch(EntryBlockSelectors);

        var translations = entryBlocks
            .SelectMany(entryBlock => entryBlock.QueryAllMatch(DefinitionBlockSelectors))
            .Select(ExtractTranslation)
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .Select(text => new TranslationResult(text!, TranslationSource.Cambridge))
            .ToList();

        return translations;
    }

    private static string? ExtractTranslation(IElement definitionBlock)
    {
        var hasDefinition = definitionBlock.QueryFirstTextMatch(DefinitionSelectors) is not null;
        return hasDefinition ? definitionBlock.QueryFirstTextMatch(TranslationSelectors) : null;
    }
}
