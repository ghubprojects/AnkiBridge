using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Dictionary.DTO;

public sealed record DictionaryEntryDetailDTO(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    IReadOnlyList<PronunciationDTO> Pronunciations,
    IReadOnlyList<DefinitionDTO> Definitions,
    IReadOnlyList<string> Images
);