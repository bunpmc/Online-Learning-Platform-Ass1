using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly List<Lesson> _lessons = new();

    private readonly Guid _module1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private readonly Guid _module2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public LessonRepository()
    {
        _lessons.Add(new Lesson
        {
            Id = Guid.NewGuid(),
            ModuleId = _module1,
            Title = "MVC là gì?",
            Type = "video",
            ContentUrl = "https://online-learning-platform.sfo3.cdn.digitaloceanspaces.com/mvclagi.mp4",
            Duration = 600,
            OrderIndex = 1
        });

        _lessons.Add(new Lesson
        {
            Id = Guid.NewGuid(),
            ModuleId = _module1,
            Title = "Cấu trúc project ASP.NET Core",
            Type = "video",
            ContentUrl = "https://online-learning-platform.sfo3.cdn.digitaloceanspaces.com/project-structure.mp4",
            Duration = 900,
            OrderIndex = 2
        });

        _lessons.Add(new Lesson
        {
            Id = Guid.NewGuid(),
            ModuleId = _module2,
            Title = "Routing cơ bản",
            Type = "text",
            ContentUrl = null,
            Duration = null,
            OrderIndex = 1
        });
    }

    public Task<IEnumerable<Lesson>> GetAllAsync()
        => Task.FromResult(_lessons.AsEnumerable());

    public Task<Lesson?> GetByIdAsync(Guid lessonId)
        => Task.FromResult(_lessons.FirstOrDefault(l => l.Id == lessonId));

    public Task<IEnumerable<Lesson>> GetByModuleIdAsync(Guid moduleId)
    {
        var lessons = _lessons
            .Where(l => l.ModuleId == moduleId)
            .OrderBy(l => l.OrderIndex)
            .AsEnumerable();

        return Task.FromResult(lessons);
    }

    public Task AddAsync(Lesson lesson)
    {
        lesson.Id = Guid.NewGuid();
        _lessons.Add(lesson);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Lesson lesson)
    {
        var existing = _lessons.FirstOrDefault(l => l.Id == lesson.Id);
        if (existing == null) return Task.CompletedTask;

        existing.Title = lesson.Title;
        existing.Type = lesson.Type;
        existing.ContentUrl = lesson.ContentUrl;
        existing.Duration = lesson.Duration;
        existing.OrderIndex = lesson.OrderIndex;
        existing.ModuleId = lesson.ModuleId;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid lessonId)
    {
        var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId);
        if (lesson != null)
            _lessons.Remove(lesson);

        return Task.CompletedTask;
    }
}
