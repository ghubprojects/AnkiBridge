using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;

namespace LexiBridge.Domain.Aggregates.Dictionary;

public sealed class EntryImage : Entity<Guid>
{
    public string Url { get; private set; } = default!;
    public ImageSource Source { get; private set; }

    private EntryImage() { }

    private EntryImage(string url, ImageSource source)
    {
        Id = Guid.CreateVersion7();
        Url = url;
        Source = source;
    }

    internal static EntryImage Create(string url, ImageSource source)
    {
        return new EntryImage(url, source);
    }
}
