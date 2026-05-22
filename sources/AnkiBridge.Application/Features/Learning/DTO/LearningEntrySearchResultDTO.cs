using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Learning.DTO;

public sealed record LearningEntrySearchResultDTO(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    string Ipa,
    string Translation,
    DateTimeOffset CreatedAt);