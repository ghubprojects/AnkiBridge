using AnkiBridge.Application.Features.Flashcard.Contracts.Anki;
using AnkiBridge.Domain.Aggregates.Flashcard.Decks;
using AnkiBridge.Shared.Results;
using MediatR;

namespace AnkiBridge.Application.Features.Flashcard.UseCases.SyncDecks;

public sealed class SyncDecksCommandHandler(
    IAnkiService ankiService,
    IDeckRepository deckRepository)
    : IRequestHandler<SyncDecksCommand, Result>
{
    public async Task<Result> Handle(SyncDecksCommand request, CancellationToken cancellationToken)
    {
        var ankiDecksResult = await ankiService.GetDecksAsync(cancellationToken);
        if (ankiDecksResult.IsFailure)
            return ankiDecksResult;

        var existingDecks = await deckRepository.ListAsync(cancellationToken);

        var ankiDecksById = ankiDecksResult.Value.ToDictionary(d => d.Id);
        var existingById = existingDecks.ToDictionary(d => d.ExternalId);

        foreach (var existing in existingDecks)
        {
            // If the deck no longer exists in Anki, delete it from the database
            if (!ankiDecksById.TryGetValue(existing.ExternalId, out var ankiDeck))
            {
                await deckRepository.DeleteAsync(existing, cancellationToken);
                continue;
            }

            // If the deck exists but has a different name, rename it in the database
            if (existing.Name != ankiDeck.Name)
                existing.Rename(ankiDeck.Name);
        }

        foreach (var ankiDeck in ankiDecksResult.Value)
        {
            if (existingById.ContainsKey(ankiDeck.Id))
                continue;

            // If the deck is new, add it to the database
            var newDeck = Deck.Create(ankiDeck.Name, ankiDeck.Id);
            await deckRepository.AddAsync(newDeck, cancellationToken);
        }

        await deckRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
