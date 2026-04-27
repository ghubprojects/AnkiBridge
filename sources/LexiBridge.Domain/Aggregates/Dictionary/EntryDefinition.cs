using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Dictionary;

public sealed class EntryDefinition : Entity<Guid>
{
    public string Text { get; private set; } = default!;
    public int OrderIndex { get; private set; }

    private readonly List<EntryExample> _examples = [];
    public IReadOnlyCollection<EntryExample> Examples => _examples.AsReadOnly();

    private EntryDefinition() { }

    private EntryDefinition(string definition, int orderIndex)
    {
        Text = definition;
        OrderIndex = orderIndex;
    }

    internal static EntryDefinition Create(string definition, int orderIndex)
    {
        return new EntryDefinition(definition, orderIndex);
    }

    internal void AddExample(string text)
    {
        _examples.Add(EntryExample.Create(text));
    }
}