using AnkiBridge.Application.Common.IntegrationEvents;
using AnkiBridge.Application.Features.Flashcard.Contracts.Anki;
using AnkiBridge.Application.Features.Flashcard.Contracts.Anki.Models;
using AnkiBridge.Domain.Aggregates.Flashcard.Notes;

namespace AnkiBridge.Application.Features.Flashcard.IntegrationEvents;

public sealed class AnkiNotesExportStartedIntegrationEventHandler(
    INoteRepository noteRepository,
    IAnkiService ankiService)
    : IIntegrationEventHandler<AnkiNotesExportStartedIntegrationEvent>
{
    public async Task Handle(AnkiNotesExportStartedIntegrationEvent integrationEvent)
    {
        var ids = integrationEvent.AnkiNoteIds
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
            return;

        var notes = await noteRepository.GetByIdsAsync(ids, includeRelated: true);

        // If notes are missing, skip processing (the command handler validated existence).
        if (notes.Count != ids.Length)
            return;

        var createDtos = notes
            .Select(ToCreateDto)
            .ToList();

        var exportResult = await ankiService.AddNotesAsync(createDtos);
        if (exportResult.IsFailure)
        {
            foreach (var note in notes)
                note.MarkAsFailed();

            await noteRepository.UnitOfWork.SaveChangesAsync();
            return;
        }

        var externalIds = exportResult.Value;

        // AnkiConnect returns an array with the same order as the submitted notes.
        for (var i = 0; i < notes.Count; i++)
        {
            var externalId = externalIds[i];

            if (externalId <= 0)
            {
                notes[i].MarkAsFailed();
                continue;
            }

            notes[i].MarkAsSuccess(externalId);
        }

        await noteRepository.UnitOfWork.SaveChangesAsync();
    }

    private static AnkiNote ToCreateDto(Note note)
    {
        var examples = note.LearningEntry.Examples
            .Select(x => x.Text)
            .ToArray();

        var example1 = examples.ElementAtOrDefault(0) ?? string.Empty;
        var example2 = examples.ElementAtOrDefault(1) ?? string.Empty;
        var example3 = examples.ElementAtOrDefault(2) ?? string.Empty;

        return new AnkiNote(
            note.NoteType.Name,
            note.Deck.Name,
            note.LearningEntry.Headword,
            note.LearningEntry.PartOfSpeech.ToString(),
            note.LearningEntry.Ipa,
            note.LearningEntry.Accent.ToString(),
            note.LearningEntry.Cloze,
            note.LearningEntry.Definition,
            note.LearningEntry.Translation,
            example1,
            example2,
            example3,
            note.LearningEntry.AudioPath ?? string.Empty,
            note.LearningEntry.ImagePath ?? string.Empty);
    }
}
