using LexiBridge.Application.Abstractions.Query.Pagination;
using LexiBridge.Application.Features.Dictionary.DTO;

namespace LexiBridge.Application.Features.Dictionary.Abstractions;

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
