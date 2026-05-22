using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.CardTemplates;

public class CardTemplate : AggregateRoot<Guid>, IAuditableEntity, ISoftDeleteEntity
{
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Css { get; private set; } = default!;

    private readonly List<CardType> _cardTypes = [];
    public IReadOnlyCollection<CardType> CardTypes => _cardTypes.AsReadOnly();

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

    private CardTemplate() { }

    private CardTemplate(string name, string description, string css)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        Css = css;
    }

    public static CardTemplate Create(string name, string description, string css)
    {
        return new CardTemplate(name, description, css);
    }

    public void Update(string name, string description, string css)
    {
        Name = name;
        Description = description;
        Css = css;
    }

    public void AddCardType(string name, string frontTemplate, string backTemplate)
    {
        _cardTypes.Add(CardType.Create(
            Id,
            name,
            frontTemplate,
            backTemplate));
    }

    public void RemoveCardType(Guid cardTypeId)
    {
        var cardType = _cardTypes.FirstOrDefault(x => x.Id == cardTypeId);
        if (cardType is null) return;

        _cardTypes.Remove(cardType);
    }
}
