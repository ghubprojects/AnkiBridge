using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Exporting.Decks;

public sealed class Deck : AggregateRoot<Guid>, IAuditableEntity, ISoftDeleteEntity
{
    public string Name { get; private set; } = default!;

    #region Audit

    public DateTimeOffset CreatedAt { get; }
    public Guid CreatedBy { get; }
    public DateTimeOffset? LastModifiedAt { get; }
    public Guid? LastModifiedBy { get; }

    #endregion

    #region Soft Delete

    public bool IsDeleted { get; }
    public DateTimeOffset? DeletedAt { get; }
    public Guid? DeletedBy { get; }

    #endregion

    private Deck() { }

    private Deck(string name)
    {
        Id = Guid.CreateVersion7();
        Name = name;
    }

    public static Deck Create(string name)
    {
        return new Deck(name);
    }

    public void Rename(string name)
    {
        Name = name;
    }
}