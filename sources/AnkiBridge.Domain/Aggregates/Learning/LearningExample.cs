using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Learning;

public sealed class LearningExample : Entity<Guid>, IAuditableEntity, ISoftDeleteEntity
{
    public string Text { get; private set; } = default!;

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

    private LearningExample() { }

    private LearningExample(string text)
    {
        Id = Guid.CreateVersion7();
        Text = text;
    }

    internal static LearningExample Create(string text)
    {
        return new LearningExample(text);
    }

    internal void UpdateText(string newText)
    {
        if (Text != newText)
            Text = newText;
    }
}