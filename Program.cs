using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

using System.ComponentModel.DataAnnotations;

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

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks().AddDbContextCheck<TodoDbContext>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Api v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapGet("/todoitems", async (TodoDbContext dbContext) => await dbContext.TodoItems.ToListAsync()).WithTags(new[] { "Read", "CRUD" });

app.MapGet("/todoitems/{id}", async (TodoDbContext dbContext, int id) =>
    await dbContext.TodoItems.FindAsync(id) is TodoItem todo ? Results.Ok(todo) : Results.NotFound()).WithTags(new[] { "Read", "ReadOne", "CRUD" });

app.MapPost("/todoitems", async (TodoDbContext dbContext, TodoItem todoItem) =>
{
    dbContext.TodoItems.Add(todoItem);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
}).WithTags(new[] { "Create", "CRUD" }).Accepts<TodoItem>("application/json").Produces(201, typeof(TodoItem));

app.MapPut("/todoitems/{id}", async (TodoDbContext dbContext, int id, TodoItem inputTodoItem) =>
{
    if (await dbContext.TodoItems.FindAsync(id) is TodoItem todoItem)
    {
        todoItem.IsCompleted = inputTodoItem.IsCompleted;
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
}).WithTags(new[] { "Update", "CRUD" }).Accepts<TodoItem>("application/json").Produces(201, typeof(TodoItem)).ProducesProblem(404);

app.MapDelete("/todoitems/{id}", async (TodoDbContext dbContext, int id) =>
{
    if (await dbContext.TodoItems.FindAsync(id) is TodoItem todoItem)
    {
        dbContext.TodoItems.Remove(todoItem);
        await dbContext.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
}).WithTags(new[] { "Delete", "CRUD" }).Accepts<TodoItem>("application/json").Produces(201, typeof(TodoItem)).ProducesProblem(404);

app.MapGet("/health", async (HealthCheckService healthCheckService) =>
{
    var report = await healthCheckService.CheckHealthAsync();
    return report.Status == HealthStatus.Healthy ? Results.Ok(report) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
}).WithTags(new[] { "Health" }).Produces(200).ProducesProblem(503);

app.MapGet("/todoitems/history", async (TodoDbContext dbContext) => await dbContext.TodoItems
    .TemporalAll()
    .OrderBy(todoItem => EF.Property<DateTime>(todoItem, "PeriodStart"))
    .Select(todoItem => new TodoItemAudit
    {
        Title = todoItem.Title,
        IsCompleted = todoItem.IsCompleted,
        PeriodStart = EF.Property<DateTime>(todoItem, "PeriodStart"),
        PeriodEnd = EF.Property<DateTime>(todoItem, "PeriodEnd")
    })
    .ToListAsync()).WithTags(new[] { "EF Core Feature" });

app.Run();

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().ToTable("TodoItems", t => t.IsTemporal());
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}

public class TodoItem
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class TodoItemAudit
{
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}