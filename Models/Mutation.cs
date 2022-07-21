using HotChocolate.Subscriptions;

using MinimalApi.Data;
using MinimalApi.ViewModels;

namespace MinimalApi.Models;
public class Mutation
{
    [UseDbContext(typeof(TodoDbContext))]
    public async Task<TodoItem> CreateTodoItem([ScopedService] TodoDbContext context,
        TodoItemInput todoItem,
        [Service] ITopicEventSender topicEventSender,
        CancellationToken cancellationToken)
    {
        var todo = new TodoItem
        {
            Title = todoItem.Title,
            IsCompleted = todoItem.IsCompleted
        };
        context.TodoItems.Add(todo);
        await context.SaveChangesAsync(cancellationToken);
        await topicEventSender.SendAsync(nameof(Subscription.OnCreateTodoItem), todo, cancellationToken);
        return todo;
    }
}
