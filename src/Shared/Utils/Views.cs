using System.Text.Encodings.Web;
using Fluid;
using Microsoft.Extensions.FileProviders;
using minimal_url_shortener.Shared.Models;

namespace minimal_url_shortener.Shared.Utils;

// Renders the Liquid templates in /Views to HTML fragments.
public sealed class Views
{
    private readonly FluidParser _parser = new();
    private readonly TemplateOptions _options = new();
    private readonly Dictionary<string, IFluidTemplate> _templates = new();

    public Views(IWebHostEnvironment env)
    {
        var directory = Path.Combine(env.ContentRootPath, "Views");
        _options.FileProvider = new PhysicalFileProvider(directory); // used by {% render 'name' %}
        _options.MemberAccessStrategy.MemberNameStrategy = MemberNameStrategies.CamelCase;
        _options.MemberAccessStrategy.Register<UrlView>();
        _options.MemberAccessStrategy.Register<ErrorView>();
        _options.MemberAccessStrategy.Register<PagedList<UrlView>>();

        foreach (var file in Directory.EnumerateFiles(directory, "*.liquid"))
            _templates[Path.GetFileNameWithoutExtension(file)] = _parser.Parse(File.ReadAllText(file));
    }

    public async Task<IResult> Render(string name, object model, int statusCode = StatusCodes.Status200OK)
    {
        // Fluid only escapes output when an encoder is passed - without HtmlEncoder every {{ value }} is raw HTML.
        var html = await _templates[name].RenderAsync(new TemplateContext(model, _options), HtmlEncoder.Default);
        return Results.Text(html, "text/html; charset=utf-8", statusCode: statusCode);
    }
}
