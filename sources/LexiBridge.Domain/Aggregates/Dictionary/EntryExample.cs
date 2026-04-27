using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Dictionary;

public sealed class EntryExample : Entity<Guid>
{
    public string Text { get; private set; } = default!;

    private EntryExample() { }

    private EntryExample(string text)
    {
        Text = text;
    }

    internal static EntryExample Create(string text)
    {
        return new EntryExample(text);
    }
}