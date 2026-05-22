using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Learning.DTO;

public sealed record LearningEntryDetailDTO(
     Guid Id,
     string Headword,
     PartOfSpeech PartOfSpeech,
     Accent Accent,
     string Ipa,
     string Cloze,
     string Definition,
     string Translation,
     string? AudioUrl,
     string? ImageUrl,
     IReadOnlyList<LearningExampleDTO> Examples,
     DateTimeOffset CreatedAt);