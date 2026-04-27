using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Dictionary;

public class DictionaryEntry : AggregateRoot<Guid>
{
    public string Headword { get; private set; } = default!;
    public PartOfSpeech PartOfSpeech { get; private set; }
    public EntrySource Source { get; private set; }

    private readonly List<Pronunciation> _pronunciations = [];
    public IReadOnlyCollection<Pronunciation> Pronunciations => _pronunciations.AsReadOnly();

    private readonly List<EntryDefinition> _definitions = [];
    public IReadOnlyCollection<EntryDefinition> Definitions => _definitions.AsReadOnly();

    private readonly List<EntryImage> _images = [];
    public IReadOnlyCollection<EntryImage> Images => _images.AsReadOnly();

    private DictionaryEntry() { }

    private DictionaryEntry(string headword, PartOfSpeech partOfSpeech, EntrySource source)
    {
        Id = Guid.CreateVersion7();
        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Source = source;
    }

    public static DictionaryEntry Create(string headword, PartOfSpeech partOfSpeech, EntrySource source)
    {
        return new DictionaryEntry(headword, partOfSpeech, source);
    }

    public void AddPronunciation(string ipa, Accent accent, string audioUrl, AudioSource audioSource)
    {
        _pronunciations.Add(Pronunciation.Create(ipa, accent, audioUrl, audioSource));
    }

    public void AddDefinition(string text, IEnumerable<string> examples)
    {
        var orderIndex = _definitions.Count + 1;
        var definition = EntryDefinition.Create(text, orderIndex);

        foreach (var example in examples)
            definition.AddExample(example);

        _definitions.Add(definition);
    }

    public void AddImage(string url, ImageSource source)
    {
        _images.Add(EntryImage.Create(url, source));
    }
}
