using LexiBridge.Application.Features.Learning.DTO;
using LexiBridge.Domain.Enums;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.UpdateLearningItem;

public sealed record UpdateLearningItemCommand(
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