namespace AnkiBridge.Shared.Results;

public sealed record ValidationError(string Property, string Code, string Message) 
    : Error(Code, Message)
{
    public ValidationError(string property, string message) 
        : this(property, "VALIDATION_ERROR", message) { }
}