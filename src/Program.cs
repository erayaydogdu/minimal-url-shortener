using HashidsNet;
using LiteDB;
using Microsoft.Extensions.FileProviders;
using minimal_url_shortener.Backend.Endpoints;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Frontend"
});
builder.Services.AddRazorComponents();
builder.Services.AddCors
(
    cors => cors
        .AddPolicy(name: "default-policy", policy => policy
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod())
);
builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>(_ => new LiteDatabase("minimal-url-shortener.db"));
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
app.AddHtmxEndpoints();
var fileOptions = new DefaultFilesOptions();
fileOptions.DefaultFileNames.Clear();
fileOptions.DefaultFileNames.Add("index.html");
fileOptions.DefaultFileNames.Add("styles.css");
fileOptions.DefaultFileNames.Add("favicon.ico");
app.UseDefaultFiles(fileOptions);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Frontend"))
});
app.UseCors("default-policy");
app.Run();