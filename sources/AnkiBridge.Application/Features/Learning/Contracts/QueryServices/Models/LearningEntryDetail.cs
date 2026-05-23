using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Learning.Contracts.QueryServices.Models;

public sealed record LearningEntryDetail(
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
     IReadOnlyList<LearningEntryDetailExample> Examples,
     DateTimeOffset CreatedAt);