using System.Collections.Immutable;

namespace Cashflow.Core.Models;

public record PaginatedList<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;


    public PaginatedList<T> FromQueryable(IQueryable<T> Source)
    {
        var count = Source.Count();
        var items = Source.Skip((Page - 1) * PageSize)
                          .Take(PageSize)
                          .ToImmutableList();

        return new PaginatedList<T>(items, count, Page, PageSize);
    }
}
