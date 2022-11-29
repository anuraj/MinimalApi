
using System.ComponentModel.DataAnnotations;
namespace MinimalApi.Models;
public class User
{
    public int Id { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? Email { get; set; }
    public ICollection<TodoItem>? Todos { get; set; }
    public string? Salt { get; set; }
    public int RateLimitWindowInMinutes { get; set; } = 5;
    public int PermitLimit { get; set; } = 60;
}
