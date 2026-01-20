using Online_Learning_Platform_Ass1.Data.Database.Entities;
public enum AiSummaryStatus
{
    None = 0,
    Pending = 1,
    Completed = 2,
    Failed = 3
}
public class LessonProgress
{
    public Guid Id { get; set; }

    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;

    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; } = null!;

    public int WatchedPosition { get; set; }
    public bool IsCompleted { get; set; }

    public string? Transcript { get; set; }
    public string? AiSummary { get; set; }
    public AiSummaryStatus AiSummaryStatus { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
