using System.Text.Json.Serialization;

namespace minimal_url_shortener.Shared.Models;

public class UrlModel()
{
    public int Id { get; set; }
    public string ShortUrl { get; set; }
    public string LongUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
}