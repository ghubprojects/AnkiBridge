using AnkiBridge.Application.Abstractions.Query.Pagination;
using AnkiBridge.Application.Features.Dictionary.DTO;

namespace AnkiBridge.Application.Features.Dictionary.Abstractions;

public interface IDictionaryEntryQueryService
{
    Task<PaginatedData<DictionaryEntrySearchResultDTO>> SearchAsync(
        string keyword,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<DictionaryEntryDetailDTO?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
}
