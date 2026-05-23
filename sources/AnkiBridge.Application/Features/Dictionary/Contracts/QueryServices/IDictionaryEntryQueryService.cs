using AnkiBridge.Application.Common.Query.Pagination;
using AnkiBridge.Application.Features.Dictionary.Contracts.QueryServices.Models;

namespace AnkiBridge.Application.Features.Dictionary.Contracts.QueryServices;

public interface IDictionaryEntryQueryService
{
    Task<PaginatedResult<DictionaryEntrySearchResult>> SearchAsync(
        string keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<DictionaryEntryDetail?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
}
