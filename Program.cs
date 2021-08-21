using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "Todo web api implementation using Minimal Api in Asp.Net Core",
    Title = "Todo Api",
    Version = "v1",
    Contact = new OpenApiContact()
    {
        Name = "anuraj",
        Url = new Uri("https://dotnetthoughts.net")
    }
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

app.MapGet("/todoitems", async ([FromServices] TodoDbContext dbContext) =>
{
    return await dbContext.TodoItems.ToListAsync();
});

app.MapGet("/todoitems/{id}", async ([FromServices] TodoDbContext dbContext, int id) =>
{
    return await dbContext.TodoItems.FindAsync(id) is TodoItem todo ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/todoitems", async ([FromServices] TodoDbContext dbContext, TodoItem todoItem) =>
{
    dbContext.TodoItems.Add(todoItem);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
});

app.MapPut("/todoitems/{id}", async ([FromServices] TodoDbContext dbContext, int id, TodoItem inputTodoItem) =>
{
    var todoItem = await dbContext.TodoItems.FindAsync(id);
    if (todoItem == null)
    {
        return Results.NotFound();
    }

    todoItem.IsCompleted = inputTodoItem.IsCompleted;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async ([FromServices] TodoDbContext dbContext, int id) =>
{
    var todoItem = await dbContext.TodoItems.FindAsync(int.Parse(id.ToString()));
    if (todoItem == null)
    {
        return Results.NotFound();
    }

    dbContext.TodoItems.Remove(todoItem);
    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Api v1");
    c.RoutePrefix = string.Empty;
});
app.Run();

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected TodoDbContext()
    {
    }
    public DbSet<TodoItem> TodoItems { get; set; }
}
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
