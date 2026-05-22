using AnkiBridge.Application.Abstractions.Services.Anki.DTO;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Application.Abstractions.Services.Anki;

public interface IAnkiService
{
    public Task<Result<List<AnkiDeckSyncDTO>>> GetDecksAsync(
        CancellationToken cancellationToken = default);

    public Task<Result<List<AnkiNoteTypeSyncDTO>>> GetNoteTypesAsync(
        CancellationToken cancellationToken = default);

    public Task<Result<List<long>>> AddNotesAsync(
        IReadOnlyList<AnkiNoteCreateDTO> notes,
        CancellationToken cancellationToken = default);
}
