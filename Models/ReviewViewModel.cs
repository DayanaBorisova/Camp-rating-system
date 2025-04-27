using System.ComponentModel.DataAnnotations;

namespace Camp_Rating_System.Models
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        // The user who submitted the review
        public string UserId { get; set; } 
        public int CampId { get; set; }

        // Rating and content
        // Rating given to the camp (1 to 5 stars)
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        // Review content (text)
        [Required]
        [StringLength(500, ErrorMessage = "Review content can't be more than 500 characters.")]
        public string Content { get; set; }

        // Date of review creation
        public DateTime CreatedDate { get; set; } 

        // Optional: If editing or updating a review, the current user’s name and the camp name
        public string UserName { get; set; }
        public string CampName { get; set; }
    }
}
