using AnkiBridge.Application.Features.Dictionary.Queries.GetDictionaryEntriesByHeadword;
using AnkiBridge.Application.Features.Dictionary.UseCases.ScrapeDictionaryEntries;
using AnkiBridge.Application.Features.Learning.UseCases.CreateLearningEntry;
using AnkiBridge.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace AnkiBridge.Web.Components.Pages.Learning;

public partial class CreateLearningItem
{
    [Inject]
    private IMediator Mediator { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private readonly LearningItemForm _form = new();
    private readonly List<DictionaryEntryCandidate> _dictionaryCandidates = [];
    private DictionaryEntryCandidate? _selectedCandidate;
    private string _searchTerm = string.Empty;
    private string? _errorMessage;
    private string? _statusMessage;
    private bool _hasSearched;
    private bool _isSearching;
    private bool _isScraping;
    private bool _isCreating;

    private IReadOnlyList<string> SelectedExamples =>
        new[] { _form.Example1, _form.Example2, _form.Example3 }
            .Where(example => !string.IsNullOrWhiteSpace(example))
            .ToList();

    private async Task SearchDictionaryAsync()
    {
        var term = _searchTerm.Trim();
        if (string.IsNullOrWhiteSpace(term))
        {
            _errorMessage = "Enter a word or phrase before searching the dictionary cache.";
            return;
        }

        _errorMessage = null;
        _statusMessage = null;
        _hasSearched = true;
        _isSearching = true;
        _dictionaryCandidates.Clear();
        _selectedCandidate = null;

        try
        {
            _dictionaryCandidates.AddRange(
                await Mediator.Send(new GetDictionaryEntriesByHeadwordQuery(term)));

            if (_dictionaryCandidates.Count > 0)
                _statusMessage = $"Found {_dictionaryCandidates.Count} cached Cambridge entr{(_dictionaryCandidates.Count == 1 ? "y" : "ies")}.";
        }
        catch (Exception)
        {
            _errorMessage = "The dictionary cache could not be searched. Please try again.";
        }
        finally
        {
            _isSearching = false;
        }
    }

    private async Task ScrapeAsync()
    {
        _errorMessage = null;
        _statusMessage = null;
        _isScraping = true;

        try
        {
            var result = await Mediator.Send(new ScrapeDictionaryEntriesCommand(_searchTerm.Trim()));
            if (result.IsFailure)
            {
                _errorMessage = result.Error.Message;
                return;
            }

            _statusMessage = "Cambridge entry scraped and saved. Choose one of the new entries below.";
            await SearchDictionaryAsync();
        }
        catch (Exception)
        {
            _errorMessage = "Cambridge could not be scraped right now. Please try again later.";
        }
        finally
        {
            _isScraping = false;
        }
    }

    private void SelectCandidate(DictionaryEntryCandidate candidate)
    {
        _selectedCandidate = candidate;
        _form.DictionaryEntryId = candidate.Id;
        _form.Headword = candidate.Headword;
        _form.PartOfSpeech = candidate.PartOfSpeech.ToString();
        _form.Cloze = GenerateCloze(candidate.Headword);

        var definition = candidate.Definitions.FirstOrDefault();
        _form.Definition = definition?.Text ?? string.Empty;
        _form.Example1 = definition?.Examples.ElementAtOrDefault(0) ?? string.Empty;
        _form.Example2 = definition?.Examples.ElementAtOrDefault(1) ?? string.Empty;
        _form.Example3 = definition?.Examples.ElementAtOrDefault(2) ?? string.Empty;

        var translation = candidate.Translations.FirstOrDefault();
        _form.Translation = translation?.Text ?? string.Empty;
        _form.TranslationSource = (translation?.Source ?? TranslationSource.User).ToString();

        var pronunciation = candidate.Pronunciations
            .FirstOrDefault(item => item.Accent == Accent.British)
            ?? candidate.Pronunciations.FirstOrDefault();
        _form.Ipa = pronunciation?.Ipa ?? string.Empty;
        _form.Accent = (pronunciation?.Accent ?? Accent.British).ToString();

        _statusMessage = "The form has been filled from the selected dictionary entry. You can edit every field.";
    }

    private async Task CreateAsync()
    {
        _errorMessage = null;
        _statusMessage = null;
        _isCreating = true;

        try
        {
            var result = await Mediator.Send(new CreateLearningEntryCommand(
                _form.Headword,
                ParsePartOfSpeech(_form.PartOfSpeech),
                _form.Cloze,
                _form.Definition,
                _form.Translation,
                ParseTranslationSource(_form.TranslationSource),
                ParseAccent(_form.Accent),
                _form.Ipa,
                _form.DictionaryEntryId,
                SelectedExamples));

            if (result.IsFailure)
            {
                _errorMessage = result.Error.Message;
                return;
            }

            Navigation.NavigateTo("/learning-items");
        }
        catch (Exception)
        {
            _errorMessage = "The learning item could not be saved. Please try again.";
        }
        finally
        {
            _isCreating = false;
        }
    }

    private void Cancel() => Navigation.NavigateTo("/learning-items");

    private static string GenerateCloze(string headword) =>
        AnkiBridge.Domain.Aggregates.Learning.LearningEntry.GenerateCloze(headword);

    private static PartOfSpeech ParsePartOfSpeech(string value) =>
        Enum.TryParse<PartOfSpeech>(value, out var partOfSpeech) ? partOfSpeech : PartOfSpeech.Other;

    private static TranslationSource ParseTranslationSource(string value) =>
        Enum.TryParse<TranslationSource>(value, out var translationSource)
            ? translationSource
            : TranslationSource.User;

    private static Accent ParseAccent(string value) =>
        Enum.TryParse<Accent>(value, out var accent) ? accent : Accent.British;

    private static string FormatEnum<TEnum>(TEnum value) where TEnum : struct, Enum =>
        string.Concat(value.ToString().Select((character, index) =>
            index > 0 && char.IsUpper(character) ? $" {character}" : character.ToString()));

    private sealed class LearningItemForm
    {
        public string Headword { get; set; } = string.Empty;
        public string PartOfSpeech { get; set; } = "Other";
        public string Cloze { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public string TranslationSource { get; set; } = "User";
        public string Accent { get; set; } = "British";
        public string Ipa { get; set; } = string.Empty;
        public Guid? DictionaryEntryId { get; set; }
        public string Example1 { get; set; } = string.Empty;
        public string Example2 { get; set; } = string.Empty;
        public string Example3 { get; set; } = string.Empty;
    }
}
