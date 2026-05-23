using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.Flashcard.Contracts.QueryServices.Models;

public sealed record NoteSearchResult(
    Guid Id,
    string Headword,
    string Deck,
    string NoteType,
    ExportStatus ExportStatus,
    DateTimeOffset CreatedAt);
