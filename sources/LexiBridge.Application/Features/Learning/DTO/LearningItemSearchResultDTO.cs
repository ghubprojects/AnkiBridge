using LexiBridge.Domain.Enums;

namespace LexiBridge.Application.Features.Learning.DTO;

public sealed record LearningItemSearchResultDTO(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    string Ipa,
    string Translation,
    DateTimeOffset CreatedAt);