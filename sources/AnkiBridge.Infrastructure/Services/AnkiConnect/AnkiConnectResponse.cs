namespace AnkiBridge.Infrastructure.Services.AnkiConnect;

public sealed record AnkiConnectResponse<T>(
    T? Result,
    string? Error
);