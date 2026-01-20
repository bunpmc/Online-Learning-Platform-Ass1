using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Learning_Platform_Ass1.Data.Database.Entities;
public class Enrollment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }

    public bool Status { get; set; } = true;
    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public ICollection<LessonProgress> Progresses { get; set; } = new List<LessonProgress>();
}
