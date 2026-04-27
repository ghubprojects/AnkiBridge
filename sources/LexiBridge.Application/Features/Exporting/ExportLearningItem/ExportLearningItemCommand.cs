using LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Exporting.ExportLearningItem;

public sealed record ExportLearningItemCommand(
    Guid LearningItemId,
    Guid DeckId,
    Guid CardTemplateId,
    ExportDestination Destination
) : IRequest<Result<Guid>>;