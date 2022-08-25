using Asp.Versioning.Conventions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MinimalApi.Models;

namespace MinimalApi;

public static class TodoApi
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder routes)
    {
        var versionSet = routes.NewApiVersionSet()
                            .HasApiVersion(1.0)
                            .HasApiVersion(2.0)
                            .ReportApiVersions()
                            .Build();

        routes.MapGet("/todoitems", GetAllTodos).Produces(200, typeof(PagedResults<TodoItemOutput>))
            .ProducesProblem(401).WithApiVersionSet(versionSet).MapToApiVersion(1.0).MapToApiVersion(2.0);
        routes.MapGet("/todoitems/{id}", GetTodo).Produces(200, typeof(TodoItemOutput))
            .ProducesProblem(401).WithApiVersionSet(versionSet);
        routes.MapPost("/todoitems", CreateTodo).Accepts<TodoItemInput>("application/json")
            .Produces(201).ProducesProblem(401).ProducesProblem(400).WithApiVersionSet(versionSet);
        routes.MapPut("/todoitems/{id}", UpdateTodo).Accepts<TodoItemInput>("application/json")
            .Produces(201).ProducesProblem(404).ProducesProblem(401).WithApiVersionSet(versionSet);
        routes.MapDelete("/todoitems/{id}", DeleteTodo)
            .Produces(204).ProducesProblem(404).ProducesProblem(401).WithApiVersionSet(versionSet);
        routes.MapPost("/bulktodoitems", BulkCreateTodo).Accepts<BulkTodoitem>("multipart/form-data")
                    .Produces(201).ProducesProblem(401).ProducesProblem(400).WithApiVersionSet(versionSet)
                    .MapToApiVersion(2.0);
        return routes;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public static async Task<IResult> BulkCreateTodo(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, BulkTodoitem bulkTodoitem)
    {
        try
        {
            var file = bulkTodoitem.File;
            if (file == null)
            {
                return Results.BadRequest("No file was provided");
            }
            
            using var dbContext = dbContextFactory.CreateDbContext();
            var currentUser = await dbContext.Users.FirstOrDefaultAsync(t => t.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            using var streamReader = new StreamReader(file.OpenReadStream());
            var todoItemsCSV = await streamReader.ReadToEndAsync();
            var todoItems = todoItemsCSV.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var todoItemsToCreate = new List<TodoItem>();
            
            foreach (var todoItem in todoItems)
            {
                var todoItemParts = todoItem.Split(',');
                var isCompleted = false;
                if (todoItemParts.Length == 2)
                {
                    isCompleted = bool.Parse(todoItemParts[1]);
                }               
                
                var todoItemToInsert = new TodoItem
                {
                    Title = todoItemParts[0],
                    IsCompleted = isCompleted,
                    User = currentUser!,
                    UserId = currentUser!.Id,
                    CreatedOn = DateTime.UtcNow
                };
                todoItemsToCreate.Add(todoItemToInsert);
            }
            
            await dbContext.TodoItems.AddRangeAsync(todoItemsToCreate);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest($"Exception occured - {ex.Message}.");
        }
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public static async Task<IResult> DeleteTodo(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, int id)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        if (await dbContext.TodoItems.FirstOrDefaultAsync(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value && t.Id == id) is TodoItem todoItem)
        {
            dbContext.TodoItems.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public static async Task<IResult> UpdateTodo(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, int id, TodoItemInput todoItemInput)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        if (await dbContext.TodoItems.FirstOrDefaultAsync(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value && t.Id == id) is TodoItem todoItem)
        {
            todoItem.IsCompleted = todoItemInput.IsCompleted;
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public static async Task<IResult> CreateTodo(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, TodoItemInput todoItemInput, IValidator<TodoItemInput> todoItemInputValidator)
    {
        var validationResult = todoItemInputValidator.Validate(todoItemInput);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
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
        return Results.Created($"/todoitems/{todoItem.Id}", new TodoItemOutput(todoItem.Title, todoItem.IsCompleted, todoItem.CreatedOn));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public static async Task<IResult> GetTodo(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, int id)
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        return await dbContext.TodoItems.FirstOrDefaultAsync(t => t.User.Username == user.FindFirst(ClaimTypes.NameIdentifier)!.Value && t.Id == id) is TodoItem todo ? Results.Ok(new TodoItemOutput(todo.Title, todo.IsCompleted, todo.CreatedOn)) : Results.NotFound();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public static async Task<IResult> GetAllTodos(IDbContextFactory<TodoDbContext> dbContextFactory, ClaimsPrincipal user, [FromQuery(Name = "page")] int? page, [FromQuery(Name = "pageSize")] int? pageSize)
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

        return Results.Ok(new PagedResults<TodoItemOutput>()
        {
            PageNumber = page.Value,
            PageSize = pageSize!.Value,
            Results = results,
            TotalNumberOfPages = totalPageCount!.Value,
            TotalNumberOfRecords = totalNumberOfRecords
        });
    }
}