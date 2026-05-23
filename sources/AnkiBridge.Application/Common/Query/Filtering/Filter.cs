namespace AnkiBridge.Application.Common.Query.Filtering;

public sealed class Filter
{
    public string Property { get; set; } = default!;
    public FilterOperator Operator { get; set; }
    public object? Value { get; set; }
}