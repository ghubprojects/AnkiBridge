using AnkiBridge.Shared.Results;
using AnkiBridge.Infrastructure.Services.AnkiConnect.Contracts;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace AnkiBridge.Infrastructure.Services.AnkiConnect;

public interface IAnkiConnectClient
{
    Task<Result<T>> SendAsync<T>(
       string action,
       object? parameters,
       CancellationToken cancellationToken);
}

public sealed class AnkiConnectClient(HttpClient httpClient) : IAnkiConnectClient
{
    private readonly int version = 6;

    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<Result<T>> SendAsync<T>(
        string action,
        object? parameters,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new AnkiConnectRequest(action, parameters ?? new { }, version);

            var json = JsonSerializer.Serialize(request, jsonSerializerOptions);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(string.Empty, stringContent, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<AnkiConnectResponse<T>>(cancellationToken);

            if (response is null)
                return Result<T>.Failure("Invalid response from Anki");

            if (response.Error is not null)
                return Result<T>.Failure($"Anki error: {response.Error}");

            return response.Result!;
        }
        catch (HttpRequestException ex) when (ex.InnerException is SocketException)
        {
            return Result<T>.Failure("Cannot connect to Anki. Please make sure Anki is running and accessible.");
        }
        catch (Exception)
        {
            return Result<T>.Failure($"An error occurred while connecting to Anki.");
        }
    }
}
