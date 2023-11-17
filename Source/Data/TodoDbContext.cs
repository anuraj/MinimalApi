using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using System.Security.Cryptography;

namespace MinimalApi.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>().ToTable("TodoItems", t => t.IsTemporal());
        modelBuilder.Entity<User>().ToTable("Users", u => u.IsTemporal());
        for (var i = 1; i <= 20; i++)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: $"secret-{i}",
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = i,
                Username = $"user{i}",
                Password = hashed,
                Email = $"user{i}@example.com",
                CreatedOn = DateTime.UtcNow,
                Salt = Convert.ToBase64String(salt),
                PermitLimit = 60,
                RateLimitWindowInMinutes = 5
            });
        }

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
