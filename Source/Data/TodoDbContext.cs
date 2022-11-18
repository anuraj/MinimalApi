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

        for (var i = 1; i <= 20; i++)
        {
            modelBuilder.Entity<TodoItem>().HasData(new TodoItem
            {
                Id = i,
                Title = $"Todo Item {i}",
                IsCompleted = false,
                CreatedOn = DateTime.UtcNow,
                UserId = 1
            });
        }
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<User> Users => Set<User>();
}
