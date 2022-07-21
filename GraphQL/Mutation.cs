using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;

using Microsoft.EntityFrameworkCore;

using MinimalApi.Data;
using MinimalApi.Models;
using MinimalApi.ViewModels;

using System.Security.Claims;

namespace MinimalApi.GraphQL;
public class Mutation
{
    [UseDbContext(typeof(TodoDbContext))]
    [Authorize]
    public async Task<TodoItem> CreateTodoItem([ScopedService] TodoDbContext dbContext,
        TodoItemInput todoItem,
        [Service] ITopicEventSender topicEventSender, [Service] IHttpContextAccessor httpContextAccessor, CancellationToken cancellationToken)
    {
        var httpUser = httpContextAccessor.HttpContext!.User;
        var user = await dbContext.Users.FirstOrDefaultAsync(t => t.Username == httpUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var todo = new TodoItem
        {
            Title = todoItem.Title,
            IsCompleted = todoItem.IsCompleted,
            User = user!,
            UserId = user!.Id,
        };

        dbContext.TodoItems.Add(todo);

        await dbContext.SaveChangesAsync(cancellationToken);
        await topicEventSender.SendAsync(nameof(Subscription.OnCreateTodoItem), todo, cancellationToken);
        return todo;
    }
}
