using AngleSharp.Dom;

namespace AnkiBridge.Infrastructure.Services.Dictionary.Cambridge.Parsing;

internal static class ParentNodeExtensions
{
    public static IElement? QueryFirstMatch(this IParentNode node, IReadOnlyList<string> fallbackSelectors)
    {
        foreach (var selector in fallbackSelectors)
        {
            var element = node.QuerySelector(selector);
            if (element is not null)
                return element;
        }

        return null;
    }

    public static string? QueryFirstTextMatch(this IParentNode node, IReadOnlyList<string> fallbackSelectors)
    {
        foreach (var selector in fallbackSelectors)
        {
            var text = node.QuerySelector(selector)?.TextContent.Trim();
            if (!string.IsNullOrWhiteSpace(text))
                return text;
        }

        return null;
    }

    public static IReadOnlyList<IElement> QueryAllMatch(this IParentNode node, IReadOnlyList<string> fallbackSelectors)
    {
        foreach (var selector in fallbackSelectors)
        {
            var elements = node.QuerySelectorAll(selector);
            if (elements.Length > 0)
                return elements;
        }

        return [];
    }
}
