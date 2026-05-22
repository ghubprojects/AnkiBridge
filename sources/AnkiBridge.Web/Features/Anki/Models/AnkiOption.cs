namespace AnkiBridge.Web.Features.Anki.Models;

public sealed class AnkiOption
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public AnkiOption(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
};