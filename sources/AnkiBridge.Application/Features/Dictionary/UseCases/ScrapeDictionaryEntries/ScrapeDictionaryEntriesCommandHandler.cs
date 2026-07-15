using AnkiBridge.Application.Abstractions.Dictionary;
using AnkiBridge.Application.Abstractions.Images;
using AnkiBridge.Application.Abstractions.Speech;
using AnkiBridge.Application.Abstractions.Translation;
using AnkiBridge.Domain.Aggregates.Dictionary;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.UseCases.ScrapeDictionaryEntries;

public sealed class ScrapeDictionaryEntriesCommandHandler(
    IDictionaryEntryProvider dictionaryProvider,
    ITranslationProvider translationProvider,
    IPhraseIpaProvider phraseIpaProvider,
    ISpeechSynthesisProvider speechSynthesisProvider,
    IImageProvider imageProvider,
    IDictionaryEntryRepository repository)
    : IRequestHandler<ScrapeDictionaryEntriesCommand, Result>
{
    public async Task<Result> Handle(
        ScrapeDictionaryEntriesCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Headword))
            return Result.Failure("Headword must not be empty.");

        var scrapeResult = await dictionaryProvider.LookupAsync(command.Headword, cancellationToken);
        if (scrapeResult.IsFailure)
            return Result.Failure(scrapeResult.Error, scrapeResult.ErrorType);

        if (scrapeResult.Value.Count == 0)
            return Result.Failure("No dictionary entries were found.");

        var translationResult = await translationProvider.TranslateAsync(command.Headword, cancellationToken);
        var translations = translationResult.IsSuccess ? translationResult.Value : [];

        var images = await FindImagesAsync(command.Headword, cancellationToken);
        var isSingleWord = command.Headword.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries).Length == 1;

        foreach (var scrapedEntry in scrapeResult.Value)
        {
            var entryResult = DictionaryEntry.Create(
                scrapedEntry.Headword,
                scrapedEntry.PartOfSpeech,
                DictionarySource.Cambridge);

            if (entryResult.IsFailure)
                return Result.Failure(entryResult.Error, entryResult.ErrorType);

            var entry = entryResult.Value;

            var populationResult = isSingleWord
                ? AddCambridgePronunciations(entry, scrapedEntry.Pronunciations)
                : await AddPhrasePronunciationsAsync(entry, command.Headword, cancellationToken);

            if (populationResult.IsFailure)
                return populationResult;

            foreach (var definition in scrapedEntry.Definitions)
            {
                populationResult = entry.AddDefinition(definition.Text, definition.Examples);
                if (populationResult.IsFailure)
                    return populationResult;
            }

            foreach (var translation in translations)
            {
                populationResult = entry.AddTranslation(translation.Text, translation.Source);
                if (populationResult.IsFailure)
                    return populationResult;
            }

            foreach (var image in images)
                entry.AddImage(image.FullUrl, image.Source);

            repository.Add(entry);
        }

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static Result AddCambridgePronunciations(
        DictionaryEntry entry,
        IReadOnlyList<DictionaryPronunciationResult> pronunciations)
    {
        foreach (var pronunciation in pronunciations)
        {
            AudioSource? audioSource = string.IsNullOrWhiteSpace(pronunciation.AudioUrl)
                ? null
                : AudioSource.Cambridge;

            var result = entry.AddPronunciation(
                pronunciation.Ipa,
                pronunciation.Accent,
                pronunciation.AudioUrl,
                audioSource);

            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }

    private async Task<Result> AddPhrasePronunciationsAsync(
        DictionaryEntry entry,
        string phrase,
        CancellationToken cancellationToken)
    {
        foreach (var accent in new[] { Accent.British, Accent.American })
        {
            var ipaResult = await phraseIpaProvider.ResolveAsync(phrase, accent, cancellationToken);
            if (ipaResult.IsFailure || string.IsNullOrWhiteSpace(ipaResult.Value))
                continue;

            var result = entry.AddPronunciation(
                ipaResult.Value,
                accent,
                speechSynthesisProvider.BuildAudioUrl(phrase),
                AudioSource.GoogleTextToSpeech);

            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }

    private async Task<IReadOnlyList<ImageResult>> FindImagesAsync(
        string headword,
        CancellationToken cancellationToken)
    {
        try
        {
            return (await imageProvider.SearchAsync(
                headword,
                count: 3,
                cancellationToken: cancellationToken))
                .Take(3)
                .ToList()
                .AsReadOnly();
        }
        catch (Exception)
        {
            return [];
        }
    }
}
