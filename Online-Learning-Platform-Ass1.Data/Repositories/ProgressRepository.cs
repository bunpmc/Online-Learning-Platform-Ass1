using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class ProgressRepository(ILessonRepository lessonRepository ) : IProgressRepository
{
    private readonly List<LessonProgress> _progresses = new();
    private readonly ILessonRepository _lessonRepository = lessonRepository;
    private int _currentId = 1;


    public async Task<LessonProgress?> GetByIdAsync(int progressId)
    {
        return await Task.Run(() =>
            _progresses.FirstOrDefault(p => p.Id == progressId)
        );
    }

    public async Task<LessonProgress?> GetByEnrollmentAndLessonAsync(int enrollmentId, int lessonId)
    {
        return await Task.Run(() =>
            _progresses.FirstOrDefault(p =>
                p.EnrollmentId == enrollmentId &&
                p.LessonId == lessonId)
        );
    }

    public async Task<IEnumerable<LessonProgress>> GetByEnrollmentIdAsync(int enrollmentId)
    {
        return await Task.Run(() =>
            _progresses
                .Where(p => p.EnrollmentId == enrollmentId)
                .AsEnumerable()
        );
    }

    public async Task AddAsync(LessonProgress progress)
    {
        await Task.Run(() =>
        {
            progress.Id = _currentId++;
            _progresses.Add(progress);
        });
    }

    public async Task UpdateAsync(LessonProgress progress)
    {
        await Task.Run(() =>
        {
            var existing = _progresses.FirstOrDefault(p => p.Id == progress.Id);
            if (existing == null) return;

            existing.EnrollmentId = progress.EnrollmentId;
            existing.LessonId = progress.LessonId;
            existing.IsCompleted = progress.IsCompleted;
            existing.WatchedPosition = progress.WatchedPosition;
        });
    }

    public async Task MarkCompletedAsync(int enrollmentId, int lessonId)
    {
        await Task.Run(() =>
        {
            var progress = _progresses.FirstOrDefault(p =>
                p.EnrollmentId == enrollmentId &&
                p.LessonId == lessonId);

            if (progress != null)
            {
                progress.IsCompleted = true;
            }
        });
    }

    public async Task UpdateWatchedPositionAsync(int enrollmentId, int lessonId, int lastWatchedPosition)
    {
        await Task.Run(() =>
        {
            var progress = _progresses.FirstOrDefault(p =>
                p.EnrollmentId == enrollmentId &&
                p.LessonId == lessonId);

            if (progress == null)
            {
                progress = new LessonProgress
                {
                    Id = _currentId++,
                    EnrollmentId = enrollmentId,
                    LessonId = lessonId,
                    WatchedPosition = lastWatchedPosition,
                    IsCompleted = false
                };
                _progresses.Add(progress);
            }
            else
            {
                progress.WatchedPosition = lastWatchedPosition;
            }
        });
    }

    public async Task DeleteAsync(int progressId)
    {
        await Task.Run(() =>
        {
            var progress = _progresses.FirstOrDefault(p => p.Id == progressId);
            if (progress != null)
            {
                _progresses.Remove(progress);
            }
        });
    }
}
