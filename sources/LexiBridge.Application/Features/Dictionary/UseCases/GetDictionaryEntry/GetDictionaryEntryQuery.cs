using LexiBridge.Application.Features.Dictionary.DTO;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Dictionary.UseCases.GetDictionaryEntry;

public sealed record GetDictionaryEntryQuery(
    Guid EntryId
) : IRequest<Result<DictionaryEntryDetailDTO>>;