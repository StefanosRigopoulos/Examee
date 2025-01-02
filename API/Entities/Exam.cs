using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Exams")]
    public class Exam
    {
        public int Id { get; set; }
        public required string ExamName { get; set; }
        public required string PublicId { get; set; }
        public required string Url { get; set; }

        // Relationship
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}