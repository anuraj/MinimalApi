using System.ComponentModel.DataAnnotations;

namespace MinimalApi.ViewModels
{
    public class UserInput
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
