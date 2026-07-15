namespace AnkiBridge.Infrastructure.Services.AnkiConnect.Contracts;

public sealed record AnkiConnectRequest(
    string Action,
    object Params,
    int Version);
