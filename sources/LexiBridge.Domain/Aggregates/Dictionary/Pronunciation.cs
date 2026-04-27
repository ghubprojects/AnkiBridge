using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Dictionary;

public class Pronunciation : Entity<Guid>
{
    public string Ipa { get; private set; } = default!;
    public Accent Accent { get; private set; }
    public string AudioUrl { get; private set; } = default!;
    public AudioSource AudioSource { get; private set; }

    private Pronunciation(string ipa, Accent accent, string audioUrl, AudioSource audioSource)
    {
        Id = Guid.CreateVersion7();
        Ipa = ipa;
        Accent = accent;
        AudioUrl = audioUrl;
        AudioSource = audioSource;
    }

    internal static Pronunciation Create(string ipa, Accent accent, string audioUrl, AudioSource audioSource)
    {
        return new Pronunciation(ipa, accent, audioUrl, audioSource);
    }
}
