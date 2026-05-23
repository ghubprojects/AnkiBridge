namespace AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;

public sealed record NoteTypeSearchResult(
    Guid Id,
    string Name);