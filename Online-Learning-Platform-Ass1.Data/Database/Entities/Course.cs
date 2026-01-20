using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;

[Table("Courses")]
public class Course
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
