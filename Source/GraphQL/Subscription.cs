using MinimalApi.Models;

namespace MinimalApi.GraphQL;
public class Subscription
{
    [Subscribe]
    [Topic]
    public TodoItem OnCreateTodoItem([EventMessage] TodoItem todoItem)
    {
        return todoItem;
    }
}
