using AnkiBridge.Application.Features.Flashcard.Contracts.Anki;
using AnkiBridge.Domain.Aggregates.Flashcard.NoteTypes;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SyncNoteTypes;

public sealed class SyncNoteTypesCommandHandler(
    IAnkiService ankiService,
    INoteTypeRepository noteTypeRepository)
    : IRequestHandler<SyncNoteTypesCommand, Result>
{
    public async Task<Result> Handle(SyncNoteTypesCommand request, CancellationToken cancellationToken)
    {
        var ankiNoteTypesResult = await ankiService.GetNoteTypesAsync(cancellationToken);
        if (ankiNoteTypesResult.IsFailure)
            return ankiNoteTypesResult;

        var existingNoteTypes = await noteTypeRepository.ListAsync(cancellationToken);

        var ankiNoteTypesById = ankiNoteTypesResult.Value.ToDictionary(d => d.Id);
        var existingById = existingNoteTypes.ToDictionary(d => d.ExternalId);

        foreach (var existing in existingNoteTypes)
        {
            // If the note type no longer exists in Anki, delete it from the local database
            if (!ankiNoteTypesById.TryGetValue(existing.ExternalId, out var ankiNoteType))
            {
                await noteTypeRepository.DeleteAsync(existing, cancellationToken);
                continue;
            }

            // If the note type exists in Anki but has a different name, update the local database
            if (existing.Name != ankiNoteType.Name)
                existing.Rename(ankiNoteType.Name);
        }

        foreach (var ankiNoteType in ankiNoteTypesResult.Value)
        {
            if (existingById.ContainsKey(ankiNoteType.Id))
                continue;

            // If the note type exists in Anki but not in the local database, add it to the local database
            var newNoteType = NoteType.Create(ankiNoteType.Name, ankiNoteType.Id);
            await noteTypeRepository.AddAsync(newNoteType, cancellationToken);
        }

        await noteTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
