using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Dictionary;

public sealed class DictionaryExample : Entity<Guid>
{
    public string Text { get; private set; } = default!;

    private DictionaryExample() { }

    private DictionaryExample(string text)
    {
        Text = text;
    }

    internal static DictionaryExample Create(string text)
    {
        return new DictionaryExample(text);
    }
}