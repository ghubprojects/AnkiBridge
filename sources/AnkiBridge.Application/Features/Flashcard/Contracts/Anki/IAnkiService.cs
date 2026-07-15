using AnkiBridge.Application.Features.Flashcard.Contracts.Anki.Models;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Features.Flashcard.Contracts.Anki;

public interface IAnkiService
{
    Task<Result<List<AnkiDeck>>> GetDecksAsync(CancellationToken cancellationToken = default);

    Task<Result<List<AnkiNoteType>>> GetNoteTypesAsync(CancellationToken cancellationToken = default);

    Task CreateDeckAsync(string deckName, CancellationToken cancellationToken);

    Task<Result<long>> AddNoteAsync(object notePayload, CancellationToken cancellationToken);

    Task<Result<List<long>>> AddNotesAsync(
        IReadOnlyList<AnkiNote> notes,
        CancellationToken cancellationToken = default);

    Task<Result<string>> StoreMediaFileFromUrlAsync(
        string filename,
        string url,
        CancellationToken cancellationToken = default);
}
