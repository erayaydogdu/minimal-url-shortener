using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;

namespace minimal_url_shortener.Shared.Utils;

public static class RazorExtensions
{
    public static RazorComponentResult Component<TComponent>(object? model = null) where TComponent : IComponent
    {
        return model is null
            ? new RazorComponentResult<TComponent>()
            : new RazorComponentResult<TComponent>(new { Model = model });
    }
}