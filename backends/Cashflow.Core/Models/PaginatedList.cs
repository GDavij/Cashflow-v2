using System.Collections.Immutable;

namespace Cashflow.Core.Models;

public record PaginatedList<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;


    public static PaginatedList<T> FromQueryable(IQueryable<T> source, int page, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToImmutableList();

        return new PaginatedList<T>(items, count, page, pageSize);
    }
}
