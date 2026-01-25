using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

public interface IAiLessonService
{
    Task<string> GenerateSummaryAsync(ProgressDTO dto);
    Task<string> AskAsync(ProgressDTO dto, string question);
}
