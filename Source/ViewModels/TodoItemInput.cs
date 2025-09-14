using System.ComponentModel.DataAnnotations;

namespace MinimalApi.ViewModels;

public class TodoItemInput : IValidatableObject
{
    [Required, MaxLength(100), MinLength(3)]
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var dbContextFactory = validationContext.GetService<IDbContextFactory<TodoDbContext>>();
        if (dbContextFactory == null)
        {
            yield break;
        }

        var dbContext = dbContextFactory.CreateDbContext();

        if (dbContext.TodoItems.Any(t => t.Title == Title))
        {
            yield return new ValidationResult("Title must be unique", [nameof(Title)]);
        }
    }
}