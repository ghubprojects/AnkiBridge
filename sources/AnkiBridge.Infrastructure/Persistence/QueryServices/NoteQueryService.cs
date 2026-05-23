using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices;
using AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.QueryServices;

public sealed class NoteQueryService(ApplicationDbContext context) : INoteQueryService
{
    public async Task<PaginatedResult<NoteSearchResult>> SearchAsync(
        string? headword,
        string? noteType,
        string? deck,
        ExportStatus? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.Notes
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
            .ThenBy(x => x.Deck.Name)
            .ThenBy(x => x.NoteType.Name)
            .ThenBy(x => x.LearningEntry.Headword)
            .ToPaginatedResultAsync(
                pageNumber,
                pageSize,
                x => new NoteSearchResult(
                    x.Id,
                    x.LearningEntry.Headword,
                    x.Deck.Name,
                    x.NoteType.Name,
                    x.Status,
                    x.CreatedAt),
                cancellationToken);
    }
}
