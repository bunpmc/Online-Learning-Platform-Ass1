using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface IProgressService
{
    Task<LessonProgress> GetLessonProgressAsync(Guid enrollmentId, Guid? lessonId);
    Task<IEnumerable<LessonProgress>> GetCourseProgressAsync(Guid enrollmentId);

    Task UpdateWatchedPositionAsync(Guid enrollmentId, Guid lessonId, int watchedPosition);
    Task CompleteLessonAsync(Guid enrollmentId, Guid lessonId);

    Task<double> GetCourseCompletionPercentageAsync(Guid enrollmentId);

}
