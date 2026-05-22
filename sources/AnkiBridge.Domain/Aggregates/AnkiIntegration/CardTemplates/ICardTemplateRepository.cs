using AnkiBridge.Domain.SeedWork;

namespace AnkiBridge.Domain.Aggregates.AnkiIntegration.CardTemplates;

public interface ICardTemplateRepository : IRepository<CardTemplate, Guid>
{
}
