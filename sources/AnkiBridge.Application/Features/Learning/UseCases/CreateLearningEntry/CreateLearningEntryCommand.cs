using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.CreateLearningEntry;

public sealed record CreateLearningEntryCommand(
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
