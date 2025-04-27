using System.ComponentModel.DataAnnotations;
using Camp_Rating_System.Data.Entities;

namespace Camp_Rating_System.Models
{
    public class CampViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        public byte[] Photo { get; set; } // image to 2MB

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
