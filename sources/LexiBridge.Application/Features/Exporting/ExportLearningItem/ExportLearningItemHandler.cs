using LexiBridge.Domain.Aggregates.Exporting.Decks;
using LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;
using LexiBridge.Shared.Results;
using MediatR;

namespace LexiBridge.Application.Features.Exporting.ExportLearningItem;

public sealed class ExportLearningItemHandler(
    ILearningItemExportRepository learningItemExportRepository,
    IDeckRepository deckRepository)
    : IRequestHandler<ExportLearningItemCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        ExportLearningItemCommand request,
        CancellationToken cancellationToken)
    {
        var deckExists = await deckRepository.ExistsAsync(request.DeckId, cancellationToken);
        if (!deckExists)
            return Result.Failure<Guid>("Deck not found.", ErrorType.NotFound);

        var export = LearningItemExport.Create(
            request.LearningItemId,
            request.DeckId,
            request.CardTemplateId,
            request.Destination
        );

        await learningItemExportRepository.AddAsync(export, cancellationToken);

        await learningItemExportRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return export.Id;
    }
}