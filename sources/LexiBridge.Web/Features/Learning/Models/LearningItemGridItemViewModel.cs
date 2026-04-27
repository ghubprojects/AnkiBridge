namespace LexiBridge.Web.Features.Learning.Models;

public sealed class LearningItemGridItemViewModel
{
    public Guid Id { get; init; }
    public string Headword { get; init; } = default!;
    public string PartOfSpeech { get; init; } = default!;
    public string Ipa { get; init; } = default!;
    public string Translation { get; init; } = default!;
    public DateTimeOffset CreatedDate { get; init; }
}
