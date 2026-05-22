using AnkiBridge.Application.Features.Learning.DTO;
using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.UpdateLearningEntry;

public sealed record UpdateLearningEntryCommand(
    Guid Id,
    string Headword,
    PartOfSpeech PartOfSpeech,
    Accent Accent,
    string Ipa,
    string Cloze,
    string Definition,
    string Translation,
    List<LearningExampleDTO> Examples,
    string? AudioUrl,
    string? ImageUrl,
    Guid? DictionaryEntryId
) : IRequest<Result>;