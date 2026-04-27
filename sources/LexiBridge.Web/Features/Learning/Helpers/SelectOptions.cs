using LexiBridge.Domain.Enums;

namespace LexiBridge.Web.Features.Learning.Helpers;

public static class SelectOptions
{
    public const int DefaultPageSize = 20;

    public static readonly int[] AllowedPageSizes =
    [
        10,
        20,
        50
    ];

    public static readonly IReadOnlyList<PartOfSpeech> OrderedPartsOfSpeech =
    [
        PartOfSpeech.Noun,
        PartOfSpeech.Verb,
        PartOfSpeech.Adjective,
        PartOfSpeech.Adverb,
        PartOfSpeech.PhrasalVerb,
        PartOfSpeech.Idiom,
        PartOfSpeech.Collocation,

        PartOfSpeech.Pronoun,
        PartOfSpeech.Preposition,
        PartOfSpeech.Conjunction,
        PartOfSpeech.Determiner,

        PartOfSpeech.AuxiliaryVerb,
        PartOfSpeech.ModalVerb,
        PartOfSpeech.Phrase,
        PartOfSpeech.Number,
        PartOfSpeech.OrdinalNumber,

        PartOfSpeech.Prefix,
        PartOfSpeech.Suffix,
    ];

    public static readonly IReadOnlyList<Accent> OrderedAccents =
    [
        Accent.American,
        Accent.British,

        Accent.Australian,
        Accent.Canadian,
        Accent.Indian,
        Accent.Irish,
        Accent.Scottish,
        Accent.SouthAfrican
    ];

    public static readonly IReadOnlyDictionary<Accent, string> AccentDisplayLabels = new Dictionary<Accent, string>
    {
        { Accent.American, "American (US)" },
        { Accent.British, "British (UK)" }
    };
}