using AnkiBridge.Application.Features.Dictionary.DTO;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Dictionary.UseCases.GetDictionaryEntry;

public sealed record GetDictionaryEntryQuery(
    Guid EntryId
) : IRequest<Result<DictionaryEntryDetailDTO>>;