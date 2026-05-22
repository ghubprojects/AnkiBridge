namespace AnkiBridge.Infrastructure.Services.Anki;

public sealed record AnkiRequest(
    string Action,
    object Params,
    int Version);