using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required] public required string UserName { get; set; }
        [Required] public required string Email { get; set; }
        [Required] [StringLength(12, MinimumLength = 6)] public required string Password { get; set; }
        [Required] public required string FirstName { get; set; }
        [Required] public required string LastName { get; set; }
        [Required] public required string Role { get; set; }
        [Required] public required string PhotoURL { get; set; }
    }
}
