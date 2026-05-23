namespace AnkiBridge.Infrastructure.Services.AnkiConnect;

public sealed record AnkiConnectRequest(
    string Action,
    object Params,
    int Version);