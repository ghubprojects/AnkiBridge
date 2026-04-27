namespace LexiBridge.Application.Abstractions.Query.Sorting;

public sealed class Sort
{
    public string Property { get; init; } = default!;
    public SortDirection Direction { get; init; } = SortDirection.Ascending;
}