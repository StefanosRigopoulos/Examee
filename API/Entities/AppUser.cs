using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Role { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public required string PhotoURL { get; set; }
        public List<Exam> Exams { get; set;} = new ();
    }
}