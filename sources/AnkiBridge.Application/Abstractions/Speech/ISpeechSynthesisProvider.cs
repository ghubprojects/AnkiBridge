namespace AnkiBridge.Application.Abstractions.Speech;

public interface ISpeechSynthesisProvider
{
    string BuildAudioUrl(string text, string language = "en");
}
