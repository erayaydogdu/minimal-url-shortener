using HashidsNet;
using LiteDB;
using minimal_url_shortener.Endpoints;
using minimal_url_shortener.Shared.Models;
using minimal_url_shortener.Shared.Utils;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Views" // index.html, favicon and the Liquid templates all live here
});

var config = builder.Configuration;
var salt = config["Hashids:Salt"] ?? throw new InvalidOperationException("Hashids:Salt is not configured.");
builder.Services.AddSingleton<IHashids>(new Hashids(salt, config.GetValue("Hashids:MinLength", 6)));
builder.Services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase(config.GetConnectionString("Db") ?? "minimal-url-shortener.db"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<ILiteDatabase>().GetCollection<UrlModel>(BsonAutoId.Int32));
builder.Services.AddSingleton<Views>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.AddHtmxEndpoints();
app.Run();
