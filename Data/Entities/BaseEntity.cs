using System.ComponentModel.DataAnnotations;

namespace Basic_System.Data.Entities
{
    public class BaseEntity
    {
            [Key]
            public int Id { get; set; }
    }
}
