public class ProgressRepository : IProgressRepository
{
    private readonly List<LessonProgress> _progresses = new();

    public Task<LessonProgress?> GetByIdAsync(Guid progressId)
        => Task.FromResult(
            _progresses.FirstOrDefault(p => p.Id == progressId)
        );

    public Task<LessonProgress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid? lessonId)
        => Task.FromResult(
            _progresses.FirstOrDefault(p =>
                p.EnrollmentId == enrollmentId &&
                p.LessonId == lessonId)
        );

    public Task<IEnumerable<LessonProgress>> GetByEnrollmentIdAsync(Guid enrollmentId)
        => Task.FromResult(
            _progresses
                .Where(p => p.EnrollmentId == enrollmentId)
                .AsEnumerable()
        );

    public Task CreateAsync(Guid enrollmentId, Guid lessonId, int watchedPosition)
    {
        _progresses.Add(new LessonProgress
        {
            Id = Guid.NewGuid(),
            EnrollmentId = enrollmentId,
            LessonId = lessonId,
            WatchedPosition = watchedPosition,
            IsCompleted = false,
            AiSummaryStatus = AiSummaryStatus.None,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        return Task.CompletedTask;
    }

    public Task CreateAndCompleteAsync(Guid enrollmentId, Guid lessonId)
    {
        _progresses.Add(new LessonProgress
        {
            Id = Guid.NewGuid(),
            EnrollmentId = enrollmentId,
            LessonId = lessonId,
            WatchedPosition = 0,
            IsCompleted = true,
            AiSummaryStatus = AiSummaryStatus.None,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        return Task.CompletedTask;
    }

    public Task UpdateWatchedPositionAsync(
        Guid enrollmentId,
        Guid lessonId,
        int watchedPosition)
    {
        var progress = _progresses.FirstOrDefault(p =>
            p.EnrollmentId == enrollmentId &&
            p.LessonId == lessonId);

        if (progress != null)
        {
            progress.WatchedPosition = watchedPosition;
            progress.UpdatedAt = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task MarkCompletedAsync(Guid enrollmentId, Guid lessonId)
    {
        var progress = _progresses.FirstOrDefault(p =>
            p.EnrollmentId == enrollmentId &&
            p.LessonId == lessonId);

        if (progress != null)
        {
            progress.IsCompleted = true;
            progress.UpdatedAt = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(LessonProgress progress)
    {
        var existing = _progresses.FirstOrDefault(p => p.Id == progress.Id);
        if (existing == null)
            return Task.CompletedTask;

        existing.WatchedPosition = progress.WatchedPosition;
        existing.IsCompleted = progress.IsCompleted;
        existing.Transcript = progress.Transcript;
        existing.AiSummary = progress.AiSummary;
        existing.AiSummaryStatus = progress.AiSummaryStatus;
        existing.UpdatedAt = DateTime.UtcNow;

        return Task.CompletedTask;
    }
}
