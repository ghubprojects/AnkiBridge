namespace AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;

public sealed record DeckSearchResult(
    Guid Id,
    string Name);