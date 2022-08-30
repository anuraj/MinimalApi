using System.Reflection;

namespace MinimalApi.ViewModels;

public class BulkTodoitem
{
    public IFormFile? File { get; set; }
    public static ValueTask<BulkTodoitem?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
    {
        var form = await context.Request.ReadFormAsync();
        var file = form.Files["file"];
        return new Photo
        {
            File = file
        };
    }
}
