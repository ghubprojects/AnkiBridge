namespace AnkiBridge.Application.Common.Query.Sorting;

public sealed class Sort
{
    public string Property { get; init; } = default!;
    public SortDirection Direction { get; init; } = SortDirection.Ascending;
}