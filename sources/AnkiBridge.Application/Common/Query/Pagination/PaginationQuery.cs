namespace AnkiBridge.Application.Common.Query.Pagination;

public abstract record PaginationQuery
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private int _pageNumber = DefaultPageNumber;
    private int _pageSize = DefaultPageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? DefaultPageNumber : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0
            ? Math.Clamp(value, 1, MaxPageSize) 
            : DefaultPageSize;
    }
}