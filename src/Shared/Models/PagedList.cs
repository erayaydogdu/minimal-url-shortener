using LiteDB;

namespace minimal_url_shortener.Shared.Models;

public class PagedList<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Items { get; set; } = new List<T>();
    
    public static PagedList<T> Create(ILiteQueryable<T> source, int page, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((page - 1) * pageSize).Limit(pageSize).ToList();
        return new PagedList<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            Items = items
        };
    }
}