using Microsoft.AspNetCore.Mvc;

namespace MinimalApi;

public static class TodoApi
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/todoitems", GetAllTodoItems).Produces(200, typeof(PagedResults<TodoItemOutput>)).ProducesProblem(401);
        routes.MapGet("/todoitems/{id}", GetTodoItemById).Produces(200, typeof(TodoItemOutput)).ProducesProblem(401);
        routes.MapPost("/todoitems", CreateTodoItem).Accepts<TodoItemInput>("application/json").Produces(201).ProducesProblem(401).ProducesProblem(400);
        routes.MapPut("/todoitems/{id}", UpdateTodoItem).Accepts<TodoItemInput>("application/json").Produces(201).ProducesProblem(404).ProducesProblem(401);
        routes.MapDelete("/todoitems/{id}", DeleteTodoItem).Produces(204).ProducesProblem(404).ProducesProblem(401);

        return routes;
    }

    public static RouteGroupBuilder MapApiEndpoints(this RouteGroupBuilder groups)
    {
        groups.MapGet("/todoitems", GetAllTodoItems).Produces(200, typeof(PagedResults<TodoItemOutput>)).ProducesProblem(401);
        groups.MapGet("/todoitems/{id}", GetTodoItemById).Produces(200, typeof(TodoItemOutput)).ProducesProblem(401);
        groups.MapPost("/todoitems", CreateTodoItem).Accepts<TodoItemInput>("application/json").Produces(201).ProducesProblem(401).ProducesProblem(400);
        groups.MapPut("/todoitems/{id}", UpdateTodoItem).Accepts<TodoItemInput>("application/json").Produces(201).ProducesProblem(404).ProducesProblem(401);
        groups.MapDelete("/todoitems/{id}", DeleteTodoItem).Produces(204).ProducesProblem(404).ProducesProblem(401);

        return groups;
    }

    public static async Task<IResult> DeleteTodoItem(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, int id)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        if (await dbContext.TodoItems.FirstOrDefaultAsync(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value && t.Id == id) is TodoItem todoItem)
        {
            dbContext.TodoItems.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    public static async Task<IResult> UpdateTodoItem(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, int id, TodoItemInput todoItemInput)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        if (await dbContext.TodoItems.FirstOrDefaultAsync(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value && t.Id == id) is TodoItem todoItem)
        {
            todoItem.IsCompleted = todoItemInput.IsCompleted;
            await dbContext.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

    public static async Task<IResult> CreateTodoItem(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, TodoItemInput todoItemInput, IValidator<TodoItemInput> todoItemInputValidator)
    {
        var validationResult = todoItemInputValidator.Validate(todoItemInput);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        using var dbContext = dbContextFactory.CreateDbContext();
        var todoItem = new TodoItem
        {
            Title = todoItemInput.Title,
            IsCompleted = todoItemInput.IsCompleted,
        };

        var currentUser = await dbContext.Users.FirstOrDefaultAsync(t => t.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        todoItem.User = currentUser!;
        todoItem.UserId = currentUser!.Id;
        todoItem.CreatedOn = DateTime.UtcNow;
        dbContext.TodoItems.Add(todoItem);
        await dbContext.SaveChangesAsync();
        return TypedResults.Created($"/todoitems/{todoItem.Id}", new TodoItemOutput(todoItem.Title, todoItem.IsCompleted, todoItem.CreatedOn));
    }

    public static async Task<IResult> GetTodoItemById(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, int id)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        return await dbContext.TodoItems.FirstOrDefaultAsync(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value && t.Id == id) is TodoItem todo ? TypedResults.Ok(new TodoItemOutput(todo.Title, todo.IsCompleted, todo.CreatedOn)) : TypedResults.NotFound();
    }

    public static async Task<IResult> GetAllTodoItems(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, [FromQuery(Name = "page")] int? page = 1, [FromQuery(Name = "pageSize")] int? pageSize = 10)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        pageSize ??= 10;
        page ??= 1;

        var skipAmount = pageSize * (page - 1);
        var queryable = dbContext.TodoItems.Where(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value).AsQueryable();
        var results = await queryable
            .Skip(skipAmount ?? 1)
            .Take(pageSize ?? 10).Select(t => new TodoItemOutput(t.Title, t.IsCompleted, t.CreatedOn)).ToListAsync();
        var totalNumberOfRecords = await queryable.CountAsync();
        var mod = totalNumberOfRecords % pageSize;
        var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);

        return TypedResults.Ok(new PagedResults<TodoItemOutput>()
        {
            PageNumber = page.Value,
            PageSize = pageSize!.Value,
            Results = results,
            TotalNumberOfPages = totalPageCount!.Value,
            TotalNumberOfRecords = totalNumberOfRecords
        });
    }
}