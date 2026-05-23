using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.Flashcard.CardTemplates;

public interface ICardTemplateRepository : IRepository<CardTemplate, Guid>
{
}
