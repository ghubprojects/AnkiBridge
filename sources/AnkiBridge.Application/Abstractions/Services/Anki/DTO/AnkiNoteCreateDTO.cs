namespace AnkiBridge.Application.Abstractions.Services.Anki.DTO;

public sealed record AnkiNoteCreateDTO(
    string NoteTypeName,
    string DeckName,
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
