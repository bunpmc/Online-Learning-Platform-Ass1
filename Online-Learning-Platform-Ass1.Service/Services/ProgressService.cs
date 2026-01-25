using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class ProgressService(IProgressRepository progressRepository) : IProgressService
{
    private readonly IProgressRepository _progressRepository = progressRepository;

    public async Task<ProgressDTO?> GetLessonProgressAsync(Guid enrollmentId, Guid? lessonId)
    {
        var progress =
            await _progressRepository.GetByEnrollmentAndLessonAsync(enrollmentId, lessonId);

        return progress == null ? null : MapToDto(progress);
    }

    public async Task<IEnumerable<ProgressDTO>> GetCourseProgressAsync(Guid enrollmentId)
    {
        var progresses =
            await _progressRepository.GetByEnrollmentIdAsync(enrollmentId);

        return progresses.Select(MapToDto);
    }

    public async Task UpdateWatchedPositionAsync(
        Guid enrollmentId,
        Guid lessonId,
        int watchedPosition)
    {
        var progress =
            await _progressRepository.GetByEnrollmentAndLessonAsync(enrollmentId, lessonId);

        if (progress == null)
        {
            await _progressRepository.CreateAsync(
                enrollmentId,
                lessonId,
                watchedPosition);
            return;
        }

        await _progressRepository.UpdateWatchedPositionAsync(
            enrollmentId,
            lessonId,
            watchedPosition);
    }

    public async Task CompleteLessonAsync(Guid enrollmentId, Guid lessonId)
    {
        var progress =
            await _progressRepository.GetByEnrollmentAndLessonAsync(enrollmentId, lessonId);

        if (progress == null)
        {
            await _progressRepository.CreateAndCompleteAsync(
                enrollmentId,
                lessonId);
            return;
        }

        await _progressRepository.MarkCompletedAsync(
            enrollmentId,
            lessonId);
    }

    public async Task<double> GetCourseCompletionPercentageAsync(Guid enrollmentId)
    {
        var progresses =
            (await _progressRepository.GetByEnrollmentIdAsync(enrollmentId)).ToList();

        if (progresses.Count == 0)
            return 0;

        return progresses.Count(p => p.IsCompleted) * 100.0 / progresses.Count;
    }


    private static ProgressDTO MapToDto(LessonProgress progress)
        => new()
        {
            Id = progress.Id,
            EnrollmentId = progress.EnrollmentId,
            LessonId = progress.LessonId,
            WatchedPosition = progress.WatchedPosition,
            AiSummary = progress.AiSummary,
            AiSummaryStatus = (DTOs.Lesson.AiSummaryStatus)progress.AiSummaryStatus,
            Transcript = progress.Transcript,
            IsCompleted = progress.IsCompleted,
            UpdatedAt = progress.UpdatedAt
        };
}

