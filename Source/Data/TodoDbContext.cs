using Microsoft.EntityFrameworkCore;

using MinimalApi.Models;

namespace MinimalApi.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().ToTable("TodoItems", t => t.IsTemporal());
        modelBuilder.Entity<User>().ToTable("Users", u => u.IsTemporal());
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            Password = "admin",
            Email = "admin@example.com",
            CreatedOn = DateTime.UtcNow
        });

        modelBuilder.Entity<TodoItem>().HasData(new TodoItem
        {
            Id = 1,
            Title = "Todo Item 1",
            IsCompleted = false,
            UserId = 1,
            CreatedOn = DateTime.UtcNow
        });

        modelBuilder.Entity<TodoItem>().HasData(new TodoItem
        {
            Id = 2,
            Title = "Todo Item 2",
            IsCompleted = false,
            UserId = 2,
            CreatedOn = DateTime.UtcNow
        });

        modelBuilder.Entity<TodoItem>().HasData(new TodoItem
        {
            Id = 3,
            Title = "Todo Item 3",
            IsCompleted = false,
            UserId = 2,
            CreatedOn = DateTime.UtcNow
        });
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<User> Users => Set<User>();
}
