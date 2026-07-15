using AnkiBridge.Domain.Enums;
using AnkiBridge.Application.Abstractions.Translation;
using AnkiBridge.Shared.Results;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AnkiBridge.Infrastructure.Services.Translation.Google;

public sealed class GoogleTranslationProvider(HttpClient httpClient, IOptions<GoogleTranslateOptions> options)
    : ITranslationProvider
{
    private readonly GoogleTranslateOptions _options = options.Value;

    public async Task<Result<IReadOnlyList<TranslationResult>>> TranslateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var requestUri = BuildRequestUri(text);

        string responseBody;

        try
        {
            responseBody = await httpClient.GetStringAsync(requestUri, cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure<IReadOnlyList<TranslationResult>>("Google Translate is unavailable.");
        }

        return ParseResponse(responseBody);
    }

    private static Result<IReadOnlyList<TranslationResult>> ParseResponse(string json)
    {
        try
        {
            using var document = JsonDocument.Parse(json);

            var segments = document.RootElement[0];

            var translatedText = string.Concat(
                segments.EnumerateArray().Select(segment => segment[0].GetString()));

            if (string.IsNullOrWhiteSpace(translatedText))
                return new List<TranslationResult>();

            return new List<TranslationResult> {
                new(translatedText, TranslationSource.GoogleTranslate) };
        }

        catch (Exception ex) when (ex is JsonException or IndexOutOfRangeException or InvalidOperationException)
        {
            return Result.Failure<IReadOnlyList<TranslationResult>>("Failed to parse Google Translate response.");
        }
    }

    private string BuildRequestUri(string text) =>
       $"{_options.BaseUrl.TrimEnd('/')}/translate_a/single" +
       $"?client=gtx&sl={_options.SourceLanguage}&tl={_options.TargetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";
}
