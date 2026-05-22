namespace AnkiBridge.Infrastructure.Services.Anki;

public sealed record AnkiResponse<T>(
    T? Result,
    string? Error
);