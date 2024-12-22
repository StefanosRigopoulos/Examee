using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Exams")]
    public class Exam
    {
        public int Id { get; set; }
        public string ExamName { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }

        // Relationship
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}