using AngleSharp;
using AngleSharp.Dom;
using AnkiBridge.Shared.Results;
using AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace AnkiBridge.Infrastructure.Services.Dictionary.Cambridge;

public abstract class CambridgeDictionaryProviderBase(IOptions<CambridgeDictionaryOptions> options) : IDisposable
{
    private readonly IBrowsingContext _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
    private readonly CambridgeDictionaryOptions _options = options.Value;

    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    protected async Task<Result<TResult>> FetchAndParseAsync<TResult>(
        string url,
        Func<IDocument, Result<TResult>> parse,
        CancellationToken cancellationToken)
    {
        try
        {
            var document = await _context.OpenAsync(url, cancellationToken);
            return parse(document);
        }
        catch (Exception)
        {
            return Result.Failure<TResult>("Cambridge Dictionary is unavailable.");
        }
    }

    protected string BuildUrl(string section, string word)
       => $"{_options.BaseUrl.TrimEnd('/')}/{section}/{Slugify(word)}";

    private static string Slugify(string word)
        => Uri.EscapeDataString(word.Trim().ToLowerInvariant().Replace(' ', '-'));

    protected string CleanText(string? text)
        => !string.IsNullOrWhiteSpace(text)
            ? WhitespaceRegex.Replace(text, " ").Trim().TrimEnd(':').Trim()
            : string.Empty;

    public void Dispose() => _context.Dispose();
}
