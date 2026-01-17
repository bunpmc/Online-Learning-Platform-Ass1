using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class ProgressService (IProgressRepository progressRepository, ILessonRepository lessonRepository) : IProgressService
{
    private readonly IProgressRepository _progressRepository = progressRepository;
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public async Task<LessonProgress?> GetLessonProgressAsync(int enrollmentId, int lessonId)
    {
        return await _progressRepository.GetByEnrollmentAndLessonAsync(enrollmentId, lessonId);
    }

    public async Task<IEnumerable<LessonProgress>> GetCourseProgressAsync(int enrollmentId)
    {
        return await _progressRepository.GetByEnrollmentIdAsync(enrollmentId);
    }

    public async Task UpdateWatchedPositionAsync(int enrollmentId, int lessonId, int watchedPosition)
    {
        await _progressRepository.UpdateWatchedPositionAsync(enrollmentId, lessonId, watchedPosition);
    }

    public async Task CompleteLessonAsync(int enrollmentId, int lessonId)
    {
        await _progressRepository.MarkCompletedAsync(enrollmentId, lessonId);
    }

    public async Task<double> GetCourseCompletionPercentageAsync(int enrollmentId)
    {
        var progresses = (await _progressRepository
            .GetByEnrollmentIdAsync(enrollmentId))
            .ToList();

        if (!progresses.Any())
            return 0;

        var completedCount = progresses.Count(p => p.IsCompleted);
        var totalCount = progresses.Count;

        return (double)completedCount / totalCount * 100;
    }

}
