using AnkiBridge.Domain.Enums;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Learning.UseCases.CreateLearningEntry;

public sealed record CreateLearningEntryCommand(
    string Headword,
    PartOfSpeech PartOfSpeech,
    string Cloze,
    string Definition,
    string Translation,
    TranslationSource TranslationSource,
    Accent Accent,
    string Ipa,
    Guid? DictionaryEntryId,
    IReadOnlyList<string> Examples) : IRequest<Result<Guid>>;
