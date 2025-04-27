using System.ComponentModel.DataAnnotations;

namespace Camp_Rating_System.Data.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        public required string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CampId { get; set; }
        public virtual Camp Camp { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
