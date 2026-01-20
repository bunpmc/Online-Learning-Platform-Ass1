using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

public class ProgressDTO
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public int LessonId { get; set; }

    public int WatchedPosition { get; set; }
    public bool IsCompleted { get; set; }

    public string? Transcript { get; set; }
    public string? AiSummary { get; set; }
    public AiSummaryStatus AiSummaryStatus { get; set; } = AiSummaryStatus.None;

    public DateTime UpdatedAt { get; set; }
}
