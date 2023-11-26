using HashidsNet;
using LiteDB;
using minimal_url_shortener.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using minimal_url_shortener.Frontend.Components;
using minimal_url_shortener.Shared.Utils;

namespace minimal_url_shortener.Backend.Endpoints;

public static class Endpoints
{
    public static void AddHtmxEndpoints(this WebApplication app)
    {
        Hashids _hashIds = new Hashids("SuperSecretSaltKey", 6);
        
        app.MapGet("/favicon.ico", () => Results.File("favicon.ico"));

        app.MapPost("/shorten", async (HttpContext context, [FromServices] ILiteDatabase _context) =>
        {
            var form = await context.Request.ReadFormAsync();
            var longUrl = form.ContainsKey("longurl") ? form["longurl"].ToString() : string.Empty;
            if (string.IsNullOrEmpty(longUrl))
                return RazorExtensions.Component<Empty>();
            var db = _context.GetCollection<UrlModel>(BsonAutoId.Int32);
            var model = new UrlModel();
            model.LongUrl = longUrl;
            model.CreatedAt = DateTime.Now;
            var id = db.Insert(model);
            model.ShortUrl = _hashIds.Encode(id);
            db.Update(model);
            return RazorExtensions.Component<UrlModelDetail>(model);
        });

        app.MapGet("/{shortUrl}", ([FromQuery] string shortUrl, [FromServices] ILiteDatabase _context) =>
        {
            var id = _hashIds.Decode(shortUrl);
            var tempId = id[0];
            var db = _context.GetCollection<UrlModel>();
            var entry = db.Query().Where(x => x.Id.Equals(tempId)).ToList().FirstOrDefault();
            if (entry != null) return Results.Redirect(entry.LongUrl);
            return RazorExtensions.Component<Empty>();
        });
        
        app.MapGet("/d/{shortUrl}", (string shortUrl, ILiteDatabase _context) =>
        {
            var id = _hashIds.Decode(shortUrl);
            var tempId = id[0];
            var db = _context.GetCollection<UrlModel>();
            var entry = db.Query().Where(x => x.Id.Equals(tempId)).ToList().FirstOrDefault();
            if (entry != null) return RazorExtensions.Component<UrlModelDetail>(entry);
            return RazorExtensions.Component<Empty>();
        });
        
    }
}