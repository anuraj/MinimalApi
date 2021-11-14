using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<TodoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

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

app.UseWebSockets();

app.MapGet("/todoitems", async (IDbContextFactory<TodoDbContext> dbContextFactory) =>
{
    using (var dbContext = dbContextFactory.CreateDbContext())
    {
        return await dbContext.TodoItems.ToListAsync();
    }
}).Produces(200, typeof(List<TodoItem>)); ;

app.MapGet("/todoitems/{id}", async (IDbContextFactory<TodoDbContext> dbContextFactory, int id) =>
{
    using (var dbContext = dbContextFactory.CreateDbContext())
    {
        return await dbContext.TodoItems.FindAsync(id) is TodoItem todo ? Results.Ok(todo) : Results.NotFound();
    }
}).Produces(200, typeof(TodoItem));

app.MapPost("/todoitems", async (IDbContextFactory<TodoDbContext> dbContextFactory, TodoItem todoItem) =>
{
    using (var dbContext = dbContextFactory.CreateDbContext())
    {

        dbContext.TodoItems.Add(todoItem);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
    }
}).Accepts<TodoItem>("application/json").Produces(201, typeof(TodoItem));

app.MapPut("/todoitems/{id}", async (IDbContextFactory<TodoDbContext> dbContextFactory, int id, TodoItem inputTodoItem) =>
{
    using (var dbContext = dbContextFactory.CreateDbContext())
    {
        if (await dbContext.TodoItems.FindAsync(id) is TodoItem todoItem)
        {
            todoItem.IsCompleted = inputTodoItem.IsCompleted;
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }
}).Accepts<TodoItem>("application/json").Produces(201, typeof(TodoItem)).ProducesProblem(404);

app.MapDelete("/todoitems/{id}", async (IDbContextFactory<TodoDbContext> dbContextFactory, int id) =>
{
    using (var dbContext = dbContextFactory.CreateDbContext())
    {
        if (await dbContext.TodoItems.FindAsync(id) is TodoItem todoItem)
        {
            dbContext.TodoItems.Remove(todoItem);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }

        return Results.NotFound();
    }
}).Accepts<TodoItem>("application/json").Produces(201, typeof(TodoItem)).ProducesProblem(404);

app.MapGet("/health", async (HealthCheckService healthCheckService) =>
{
    var report = await healthCheckService.CheckHealthAsync();
    return report.Status == HealthStatus.Healthy ? Results.Ok(report) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
}).WithTags(new[] { "Health" }).Produces(200).ProducesProblem(503);

app.MapGet("/todoitems/history", async (IDbContextFactory<TodoDbContext> dbContextFactory) =>
{
    using (var dbContext = dbContextFactory.CreateDbContext())
    {

        await dbContext.TodoItems
    .TemporalAll()
    .OrderBy(todoItem => EF.Property<DateTime>(todoItem, "PeriodStart"))
    .Select(todoItem => new TodoItemAudit
    {
        Title = todoItem.Title,
        IsCompleted = todoItem.IsCompleted,
        PeriodStart = EF.Property<DateTime>(todoItem, "PeriodStart"),
        PeriodEnd = EF.Property<DateTime>(todoItem, "PeriodEnd")
    })
    .ToListAsync();
    }
}).WithTags(new[] { "EF Core Feature" });

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

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

[GraphQLDescription("A todo item")]
public class TodoItem
{
    public int Id { get; set; }
    [Required]
    [GraphQLDescription("The title of the todo item")]
    public string? Title { get; set; }
    [GraphQLDescription("The completed status of the todo item")]
    public bool IsCompleted { get; set; }
}

public record TodoItemInput(string? Title, bool IsCompleted);

public class TodoItemAudit
{
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class Query
{
    [UseDbContext(typeof(TodoDbContext))]
    public IQueryable<TodoItem> GetTodoItems([ScopedService] TodoDbContext context) =>
            context.TodoItems;
}

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

public class Subscription
{
    [Subscribe]
    [Topic]
    public TodoItem OnCreateTodoItem([EventMessage] TodoItem todoItem)
    {
        return todoItem;
    }
}