using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Dictionary.DTO;

public sealed record DictionaryEntrySearchResultDTO(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech
);