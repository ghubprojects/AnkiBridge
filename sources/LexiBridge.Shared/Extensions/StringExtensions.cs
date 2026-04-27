namespace LexiBridge.Shared.Extensions;

public static class StringExtensions
{
    public static string WrapWithSlashes(this string text)
    {
        return string.IsNullOrWhiteSpace(text)
            ? string.Empty
            : $"/{text.Trim('/')}/";
    }
}
