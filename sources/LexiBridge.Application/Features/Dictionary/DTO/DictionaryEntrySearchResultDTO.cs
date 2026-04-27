using LexiBridge.Domain.Enums;

namespace LexiBridge.Application.Features.Dictionary.DTO;

public sealed record DictionaryEntrySearchResultDTO(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech
);