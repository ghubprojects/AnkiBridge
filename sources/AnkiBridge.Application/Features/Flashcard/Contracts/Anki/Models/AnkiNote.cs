namespace AnkiBridge.Application.Features.Flashcard.Contracts.Anki.Models;

public sealed record AnkiNote(
    string DeckName,
    string NoteTypeName,
    string Headword,
    string PartOfSpeech,
    string Ipa,
    string Accent,
    string Cloze,
    string Definition,
    string Translation,
    string Example1,
    string Example2,
    string Example3,
    string AudioPath,
    string ImagePath);
