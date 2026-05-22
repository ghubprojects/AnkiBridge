using AnkiBridge.Application.Abstractions.Services.Anki;
using AnkiBridge.Application.Abstractions.Services.Anki.DTO;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Infrastructure.Services.Anki;

public sealed class AnkiService(IAnkiClient client) : IAnkiService
{
    public async Task<Result<List<AnkiDeckSyncDTO>>> GetDecksAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await client.SendAsync<Dictionary<string, long>>(
            "deckNamesAndIds",
            null,
            cancellationToken);

        if (result.IsFailure)
            return result.ToFailure<List<AnkiDeckSyncDTO>>();

        var decks = result.Value
            .Select(kvp => new AnkiDeckSyncDTO(kvp.Value, kvp.Key))
            .ToList();

        return decks;
    }

    public async Task<Result<List<AnkiNoteTypeSyncDTO>>> GetNoteTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await client.SendAsync<Dictionary<string, long>>(
            "modelNamesAndIds",
            null,
            cancellationToken);

        if (result.IsFailure)
            return result.ToFailure<List<AnkiNoteTypeSyncDTO>>();

        var noteTypes = result.Value
            .Select(kvp => new AnkiNoteTypeSyncDTO(kvp.Value, kvp.Key))
            .ToList();

        return noteTypes;
    }

    public async Task CreateDeckAsync(string deckName, CancellationToken cancellationToken)
    {
        await client.SendAsync<object>(
            "createDeck",
            new { deck = deckName },
            cancellationToken);
    }

    public async Task<Result<long>> AddNoteAsync(object notePayload, CancellationToken cancellationToken)
    {
        var result = await client.SendAsync<long>(
            "addNote",
            new { note = notePayload },
            cancellationToken);

        return result;
    }

    public async Task<Result<List<long>>> AddNotesAsync(
        IReadOnlyList<AnkiNoteCreateDTO> notes, 
        CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            notes = notes.Select(x => new
            {
                deckName = x.DeckName,
                modelName = x.NoteTypeName,
                fields = new
                {
                    x.Headword,
                    x.PartOfSpeech,
                    x.Ipa,
                    x.Accent,
                    x.Cloze,
                    x.Definition,
                    x.Translation,
                    x.Example1,
                    x.Example2,
                    x.Example3,
                    x.AudioPath,
                    x.ImagePath
                }
            })
        };

        var result = await client.SendAsync<long[]>(
            "addNotes", 
            payload, 
            cancellationToken);

        if (result.IsFailure)
            return result.ToFailure<List<long>>();

        var externalNoteIds = result.Value.ToList();

        return externalNoteIds;
    }
}
