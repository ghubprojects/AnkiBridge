namespace AnkiBridge.Web.Features.Flashcard.Models;

public sealed record CreateNotesDialogContent(IReadOnlyList<Guid> LearningEntryIds);
