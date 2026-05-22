using AnkiBridge.Domain.Enums;

namespace AnkiBridge.Application.Features.AnkiIntegration.DTO;

public sealed record AnkiNoteSearchResultDTO(
    Guid Id,
    Guid LearningEntryId,
    string LearningEntryHeadword,
    Guid NoteTypeId,
    string NoteTypeName,
    Guid DeckId,
    string DeckName,
    ExportStatus ExportStatus,
    DateTimeOffset CreatedAt);
