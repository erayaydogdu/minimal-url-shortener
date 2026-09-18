using HashidsNet;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using minimal_url_shortener.Shared.Models;
using minimal_url_shortener.Shared.Utils;

namespace minimal_url_shortener.Endpoints;

public static class Endpoints
{
    private const int PageSize = 10;

    public static void AddHtmxEndpoints(this WebApplication app)
    {
        app.MapGet("/history", (HttpContext http, ILiteCollection<UrlModel> urls, IHashids hashids, Views views,
            [FromQuery(Name = "p")] string? p) =>
        {
            _ = int.TryParse(p, out var page); // junk falls back to page 1 (PagedList clamps it)
            return RenderList(http, urls, hashids, views, page);
        });

        app.MapPost("/shorten", async (HttpContext http, ILiteCollection<UrlModel> urls, IHashids hashids, Views views,
            [FromForm] string? longUrl) =>
        {
            longUrl = longUrl?.Trim();
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out var uri) || uri.Scheme is not ("http" or "https"))
                return await views.Render("error", new ErrorView("Please enter a valid http(s) URL."),
                    StatusCodes.Status422UnprocessableEntity);

            urls.Insert(new UrlModel { LongUrl = longUrl!, CreatedAt = DateTime.UtcNow });
            return await RenderList(http, urls, hashids, views, page: 1);
        }).DisableAntiforgery();

        // Static files are skipped for any request that matches an endpoint, so this catch-all only accepts
        // word characters (no dots); otherwise it would swallow /favicon.ico and /index.html.
        app.MapGet("/{code:regex(^\\w+$)}", (string code, ILiteCollection<UrlModel> urls, IHashids hashids) =>
            hashids.TryDecodeSingle(code, out var id) && urls.FindById(id) is { } entry
                ? Results.Redirect(entry.LongUrl)
                : Results.NotFound());
    }

    private static Task<IResult> RenderList(HttpContext http, ILiteCollection<UrlModel> urls, IHashids hashids,
        Views views, int page)
    {
        var baseUrl = UrlExtensions.GetAppUrl(http.Request);
        var model = PagedList<UrlModel>
            .Create(urls.Query().OrderByDescending(x => x.Id), page, PageSize)
            .Map(x => new UrlView(baseUrl + hashids.Encode(x.Id), x.LongUrl));
        return views.Render("url-list", model);
    }
}
