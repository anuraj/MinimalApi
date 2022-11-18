
using System.ComponentModel.DataAnnotations;
namespace MinimalApi.Models;
[GraphQLDescription("A todo item")]
public class TodoItem
{
    public int Id { get; set; }
    [Required]
    [GraphQLDescription("The title of the todo item")]
    public string? Title { get; set; }
    [GraphQLDescription("The completed status of the todo item")]
    public bool IsCompleted { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }
    public DateTime CreatedOn { get; set; }
}
