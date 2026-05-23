using AnkiBridge.Application.Features.Flashcard.Contracts.Anki.Models;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Features.Flashcard.Contracts.Anki;

public interface IAnkiService
{
    public Task<Result<List<AnkiDeck>>> GetDecksAsync(
        CancellationToken cancellationToken = default);

    public Task<Result<List<AnkiNoteType>>> GetNoteTypesAsync(
        CancellationToken cancellationToken = default);

    public Task<Result<List<long>>> AddNotesAsync(
        IReadOnlyList<AnkiNote> notes,
        CancellationToken cancellationToken = default);
}
