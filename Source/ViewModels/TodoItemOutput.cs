namespace MinimalApi.ViewModels;

public class TodoItemOutput(string title, bool isCompleted, DateTime createdOn)
{
    public string Title { get; set; } = title;
    public bool IsCompleted { get; set; } = isCompleted;
    public DateTime CreatedOn { get; set; } = createdOn;
}