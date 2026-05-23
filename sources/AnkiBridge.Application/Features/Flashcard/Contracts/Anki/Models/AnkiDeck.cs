namespace AnkiBridge.Application.Features.Flashcard.Contracts.Anki.Models;

public sealed record AnkiDeck(
    long Id,
    string Name);