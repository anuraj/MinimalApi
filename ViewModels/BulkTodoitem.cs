using System.Reflection;

namespace MinimalApi.ViewModels;

public class BulkTodoitem
{
    public IFormFile? File { get; set; }
    public static ValueTask<BulkTodoitem?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
    {
        var file = context.Request.Form.Files["File"];
        return ValueTask.FromResult<BulkTodoitem?>(new BulkTodoitem
        {
            File = file
        });
    }
}