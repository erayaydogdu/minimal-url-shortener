namespace minimal_url_shortener.Shared.Utils;

public static class UrlExtensions
{
    // "https://host[:port]/" for the current request; the port is omitted when it is the default one.
    public static string GetAppUrl(HttpRequest request) => $"{request.Scheme}://{request.Host}/";
}
