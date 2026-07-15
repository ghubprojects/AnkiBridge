namespace AnkiBridge.Infrastructure.Services.Translation.Google;

public sealed class GoogleTranslateOptions
{
    public const string SectionName = "Translation:Google";

    public string BaseUrl { get; set; } = "https://translate.googleapis.com";
    public string SourceLanguage { get; set; } = "en";
    public string TargetLanguage { get; set; } = "vi";
    public int TimeoutSeconds { get; set; } = 10;
}
