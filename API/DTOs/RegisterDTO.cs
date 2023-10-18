using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required] public string UserName { get; set; }
        [Required] public string Email { get; set; }
        [Required] [StringLength(12, MinimumLength = 6)] public string Password { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string Role { get; set; }
        [Required] public DateOnly? DateOfBirth { get; set; }
        [Required] public string PhotoURL { get; set; }
    }
}
