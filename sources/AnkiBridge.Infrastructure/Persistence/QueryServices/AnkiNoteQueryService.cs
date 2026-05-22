using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.AnkiIntegration.Abstractions;
using AnkiBridge.Application.Features.AnkiIntegration.DTO;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.QueryServices;

public sealed class AnkiNoteQueryService(
    ApplicationDbContext context) 
    : IAnkiNoteQueryService
{
    public async Task<PaginatedData<AnkiNoteSearchResultDTO>> SearchAsync(
        string? headword,
        string? noteType,
        string? deck,
        ExportStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.AnkiNotes
            .AsNoTracking()
            .Include(x => x.LearningEntry)
            .Include(x => x.NoteType)
            .Include(x => x.Deck)
            .AsSplitQuery();

        if (!string.IsNullOrWhiteSpace(headword))
            query = query.Where(x => EF.Functions.Like(x.LearningEntry.Headword, $"%{headword}%"));

        if (!string.IsNullOrWhiteSpace(noteType))
            query = query.Where(x => EF.Functions.Like(x.NoteType.Name, $"%{noteType}%"));

        if (!string.IsNullOrWhiteSpace(deck))
            query = query.Where(x => EF.Functions.Like(x.Deck.Name, $"%{deck}%"));

        if (status is not null)
            query = query.Where(x => x.Status == status);

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .ToPaginatedDataAsync(
                pageNumber,
                pageSize,
                x => new AnkiNoteSearchResultDTO(
                    x.Id,
                    x.LearningEntryId,
                    x.LearningEntry.Headword,
                    x.NoteTypeId,
                    x.NoteType.Name,
                    x.DeckId,
                    x.Deck.Name,
                    x.Status,
                    x.CreatedAt),
                cancellationToken);
    }
}
