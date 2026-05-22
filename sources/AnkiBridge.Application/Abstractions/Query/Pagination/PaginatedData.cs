namespace AnkiBridge.Application.Abstractions.Query.Pagination;

public class PaginatedData<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;

    private PaginatedData(
        IReadOnlyList<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static PaginatedData<T> Create(
        IEnumerable<T> source,
        int pageNumber,
        int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageNumber);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

        var enumerable = source as IList<T> ?? [.. source];
        var totalCount = enumerable.Count;
        var items = enumerable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedData<T>(items, pageNumber, pageSize, totalCount);
    }

    public static PaginatedData<T> Create(
        IReadOnlyList<T> items,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PaginatedData<T>(items, pageNumber, pageSize, totalCount);
    }
}