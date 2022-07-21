using HotChocolate.AspNetCore.Authorization;

using Microsoft.EntityFrameworkCore;

using MinimalApi.Data;
using MinimalApi.Models;

using System.Security.Claims;

namespace MinimalApi.GraphQL;
public class Query
{
    [UseDbContext(typeof(TodoDbContext))]
    [Authorize]
    public async Task<IQueryable<TodoItem>> GetTodoItems([ScopedService] TodoDbContext dbContext, [Service] IHttpContextAccessor httpContextAccessor)
    {
        var httpUser = httpContextAccessor.HttpContext!.User;
        var user = await dbContext.Users.FirstOrDefaultAsync(t => t.Username == httpUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        return dbContext.TodoItems;
    }
}
