using FluentValidation;

namespace AnkiBridge.Application.Features.Learning.UseCases.CreateLearningEntry;

public sealed class CreateLearningEntryCommandValidator : AbstractValidator<CreateLearningEntryCommand>
{
    public CreateLearningEntryCommandValidator()
    {
        RuleFor(x => x.Headword)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.PartOfSpeech)
            .IsInEnum();

        RuleFor(x => x.Accent)
            .IsInEnum();

        RuleFor(x => x.Ipa)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Cloze)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Definition)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Translation)
            .NotEmpty()
            .MaximumLength(100);

        RuleForEach(x => x.Examples)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Examples)
            .Must(x => x.Count <= 5);
    }
}