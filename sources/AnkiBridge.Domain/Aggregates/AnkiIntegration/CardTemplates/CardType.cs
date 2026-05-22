using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.CardTemplates;

public sealed class CardType : Entity<Guid>, IAuditableEntity, ISoftDeleteEntity
{
    public Guid CardTemplateId { get; private set; }
    public string Name { get; private set; } = default!;
    public string FrontHtml { get; private set; } = default!;
    public string BackHtml { get; private set; } = default!;

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

    private CardType() { }

    private CardType(
        Guid cardTemplateId,
        string name,
        string frontHtml,
        string backHtml)
    {
        Id = Guid.CreateVersion7();
        CardTemplateId = cardTemplateId;
        Name = name;
        FrontHtml = frontHtml;
        BackHtml = backHtml;
    }

    internal static CardType Create(
        Guid cardTemplateId,
        string name,
        string frontHtml,
        string backHtml)
    {
        return new CardType(
            cardTemplateId,
            name,
            frontHtml,
            backHtml);
    }

    internal void Update(string frontHtml, string backHtml)
    {
        FrontHtml = frontHtml;
        BackHtml = backHtml;
    }
}