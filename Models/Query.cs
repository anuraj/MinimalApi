using MinimalApi.Data;

namespace MinimalApi.Models;
public class Query
{
    [UseDbContext(typeof(TodoDbContext))]
    public IQueryable<TodoItem> GetTodoItems([ScopedService] TodoDbContext context) =>
            context.TodoItems;
}
