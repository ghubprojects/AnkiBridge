using LexiBridge.Domain.Enums;

namespace LexiBridge.Application.Features.Dictionary.DTO;

public sealed record DictionaryEntryDetailDTO(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    IReadOnlyList<PronunciationDTO> Pronunciations,
    IReadOnlyList<DefinitionDTO> Definitions,
    IReadOnlyList<string> Images
);