using AnkiBridge.Application.Abstractions.Query.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AnkiBridge.Infrastructure.Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static async Task<PaginatedData<TResult>> ToPaginatedDataAsync<T, TResult>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return PaginatedData<TResult>.Create(items, pageNumber, pageSize, totalCount);
    }
}
