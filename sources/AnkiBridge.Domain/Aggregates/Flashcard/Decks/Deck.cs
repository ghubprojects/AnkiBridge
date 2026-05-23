using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Flashcard.Decks;

public sealed class Deck : AggregateRoot<Guid>, IAuditableEntity, ISoftDeleteEntity
{
    public string Name { get; private set; } = default!;
    public long ExternalId { get; private set; }

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

    private Deck(string name, long externalId)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        ExternalId = externalId;
    }

    public static Deck Create(string name, long externalId)
    {
        return new Deck(name, externalId);
    }

    public void Rename(string name)
    {
        Name = name;
    }
}