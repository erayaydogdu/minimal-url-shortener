namespace minimal_url_shortener.Shared.Models;

// The class name doubles as the LiteDB collection name, so don't rename it.
public class UrlModel
{
    public int Id { get; set; }
    public string LongUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
