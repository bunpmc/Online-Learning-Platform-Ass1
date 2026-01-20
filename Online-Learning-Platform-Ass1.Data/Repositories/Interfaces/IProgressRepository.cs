public interface IProgressRepository
{
    Task<LessonProgress?> GetByIdAsync(Guid progressId);

    Task<LessonProgress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid? lessonId);
    Task<IEnumerable<LessonProgress>> GetByEnrollmentIdAsync(Guid enrollmentId);

    Task CreateAsync(Guid enrollmentId, Guid lessonId, int watchedPosition);
    Task CreateAndCompleteAsync(Guid enrollmentId, Guid lessonId);

    Task UpdateWatchedPositionAsync(Guid enrollmentId, Guid lessonId, int watchedPosition);
    Task MarkCompletedAsync(Guid enrollmentId, Guid lessonId);

    Task UpdateAsync(LessonProgress progress);
}
