using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Domain.Aggregates.Learning;

public sealed class LearningExample : Entity<Guid>
{
    public string Text { get; private set; } = default!;
    public int OrderIndex { get; private set; }

    private LearningExample() { }

    private LearningExample(string text, int orderIndex)
    {
        Id = Guid.CreateVersion7();
        Text = text;
        OrderIndex = orderIndex;
    }

    internal static Result<LearningExample> Create(string text, int orderIndex)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Result.Failure<LearningExample>("Example text must not be empty.");

        return new LearningExample(text.Trim(), orderIndex);
    }
}
