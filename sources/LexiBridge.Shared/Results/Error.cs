namespace LexiBridge.Shared.Results;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public Error(string message) : this("ERROR", message) { }

    public bool IsNone => this == None;
}