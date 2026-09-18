using LiteDB;

namespace minimal_url_shortener.Shared.Models;

public class PagedList<T>
{
    private const int PageWindow = 5;

    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public List<T> Items { get; init; } = [];

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    // Page numbers to show in the pager: a window of PageWindow pages around the current one.
    public List<int> Pages
    {
        get
        {
            var first = Math.Max(1, Math.Min(Page - PageWindow / 2, TotalPages - PageWindow + 1));
            var last = Math.Min(TotalPages, first + PageWindow - 1);
            return Enumerable.Range(first, last - first + 1).ToList();
        }
    }

    // An out-of-range page is clamped rather than rejected.
    public static PagedList<T> Create(ILiteQueryable<T> source, int page, int pageSize)
    {
        var count = source.Count();
        var totalPages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
        page = Math.Clamp(page, 1, totalPages);
        return new PagedList<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = count,
            TotalPages = totalPages,
            Items = source.Skip((page - 1) * pageSize).Limit(pageSize).ToList()
        };
    }

    public PagedList<TOut> Map<TOut>(Func<T, TOut> selector) => new()
    {
        Page = Page,
        PageSize = PageSize,
        TotalCount = TotalCount,
        TotalPages = TotalPages,
        Items = Items.Select(selector).ToList()
    };
}
