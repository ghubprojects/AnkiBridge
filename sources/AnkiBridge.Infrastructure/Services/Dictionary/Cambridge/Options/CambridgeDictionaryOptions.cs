namespace AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Options;

public sealed class CambridgeDictionaryOptions
{
    public const string SectionName = "Dictionary:Cambridge";

    public string BaseUrl { get; set; } = "https://dictionary.cambridge.org/dictionary";
    public int TimeoutSeconds { get; set; } = 30;
}
