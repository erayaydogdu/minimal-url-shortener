namespace minimal_url_shortener.Shared.Models;

// Read-only shapes handed to the Liquid templates. Register new ones in Views.
public record UrlView(string ShortUrl, string LongUrl);

public record ErrorView(string Message);
