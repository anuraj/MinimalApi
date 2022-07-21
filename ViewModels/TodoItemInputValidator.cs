using FluentValidation;

using Microsoft.EntityFrameworkCore;

using MinimalApi.Data;

namespace MinimalApi.ViewModels;

public class TodoItemInputValidator : AbstractValidator<TodoItemInput>
{
    private readonly TodoDbContext _todoDbContext;
    public TodoItemInputValidator(IDbContextFactory<TodoDbContext> dbContextFactory)
    {
        _todoDbContext = dbContextFactory.CreateDbContext();

        RuleFor(t => t.Title).NotEmpty().MaximumLength(100).MinimumLength(3)
            .Must(IsUnique).WithMessage("Title should be unique.");
    }

    private bool IsUnique(string title)
    {
        return !_todoDbContext.TodoItems.Any(t => t.Title == title);
    }
}
