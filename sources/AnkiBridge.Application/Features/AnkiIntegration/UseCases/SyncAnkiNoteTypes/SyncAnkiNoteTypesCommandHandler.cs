using AnkiBridge.Application.Abstractions.Services.Anki;
using AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.AnkiIntegration.UseCases.SyncAnkiNoteTypes;

public sealed class SyncAnkiNoteTypesCommandHandler(
    IAnkiService ankiService,
    IAnkiNoteTypeRepository noteTypeRepository)
    : IRequestHandler<SyncAnkiNoteTypesCommand, Result>
{
    public async Task<Result> Handle(SyncAnkiNoteTypesCommand request, CancellationToken cancellationToken)
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
            var newNoteType = AnkiNoteType.Create(ankiNoteType.Name, ankiNoteType.Id);
            await noteTypeRepository.AddAsync(newNoteType, cancellationToken);
        }

        await noteTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
