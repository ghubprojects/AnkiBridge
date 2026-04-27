using LexiBridge.Application.Features.Dictionary.Abstractions;
using LexiBridge.Application.Features.Dictionary.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Dictionary.UseCases.GetDictionaryEntry;

public sealed class GetDictionaryEntryQueryHandler(
    IDictionaryEntryQueryService queryService)
    : IRequestHandler<GetDictionaryEntryQuery, Result<DictionaryEntryDetailDTO>>
{
    public async Task<Result<DictionaryEntryDetailDTO>> Handle(GetDictionaryEntryQuery request, CancellationToken cancellationToken)
    {
        var entry = await queryService.GetAsync(request.EntryId, cancellationToken);
        if (entry is null)
            return Result<DictionaryEntryDetailDTO>.Failure("Dictionary entry not found.", ErrorType.NotFound);

        return entry;
    }
}