using System.ComponentModel.DataAnnotations;
using Basic_System.Data.Entities;

namespace Camp_Rating_System.Data.Entities
{
    public class Camp : BaseEntity
    {
        [Required, MaxLength(64)]
        public required string Name { get; set; }

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
