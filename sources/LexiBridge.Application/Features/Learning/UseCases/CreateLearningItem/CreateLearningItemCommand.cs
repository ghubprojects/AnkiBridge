using LexiBridge.Domain.Enums;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Learning.UseCases.CreateLearningItem;

public sealed record CreateLearningItemCommand(
    string Headword,
    PartOfSpeech PartOfSpeech,
    string Ipa,
    Accent Accent,
    string Cloze,
    string Definition,
    string Translation,
    List<string> Examples,
    
    Stream? AudioStream,
    string? AudioFileName,
    string? AudioContentType,

    Stream? ImageStream,
    string? ImageFileName,
    string? ImageContentType,

    Guid? DictionaryEntryId
) : IRequest<Result<Guid>>;
