namespace AnkiBridge.Infrastructure.Services.AnkiConnect.Contracts;

public sealed record AnkiConnectResponse<T>(
    T? Result,
    string? Error
);
