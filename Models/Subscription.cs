namespace MinimalApi.Models;
public class Subscription
{
    [Subscribe]
    [Topic]
    public TodoItem OnCreateTodoItem([EventMessage] TodoItem todoItem)
    {
        return todoItem;
    }
}
