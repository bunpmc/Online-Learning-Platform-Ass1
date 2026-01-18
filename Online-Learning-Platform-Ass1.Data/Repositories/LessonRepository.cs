using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class LessonRepository : ILessonRepository
{
    private readonly List<Lesson> _lessons = new();
    private int _currentId = 1;

    public LessonRepository()
    {
        _lessons.Add(new Lesson
        {
            Id = _currentId++,
            Title = "MVC là gì?",
            Content = "Giới thiệu mô hình MVC",
            VideoUrl = "https://online-learning-platform.sfo3.cdn.digitaloceanspaces.com/mvclagi.mp4",
            Duration = 10,
            OrderIndex = 1,
            ModuleId = 1
        });

        _lessons.Add(new Lesson
        {
            Id = _currentId++,
            Title = "Cấu trúc project ASP.NET Core",
            Content = "Tìm hiểu thư mục Controllers, Views, Models",
            VideoUrl = "https://online-learning-platform.sfo3.cdn.digitaloceanspaces.com/mvclagi.mp4",
            Duration = 15,
            OrderIndex = 2,
            ModuleId = 1
        });

        _lessons.Add(new Lesson
        {
            Id = _currentId++,
            Title = "Routing cơ bản",
            Content = "Attribute routing và convention routing",
            VideoUrl = "",
            Duration = 12,
            OrderIndex = 1,
            ModuleId = 2
        });
    }

    public Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return Task.FromResult(_lessons.AsEnumerable());
    }

    public Task<Lesson?> GetByIdAsync(int lessonId)
    {
        var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId);
        return Task.FromResult(lesson);
    }

    public Task<IEnumerable<Lesson>> GetByModuleIdAsync(int moduleId)
    {
        var lessons = _lessons
            .Where(l => l.ModuleId == moduleId)
            .OrderBy(l => l.OrderIndex)
            .AsEnumerable();

        return Task.FromResult(lessons);
    }

    public Task AddAsync(Lesson lesson)
    {
        lesson.Id = _currentId++;
        _lessons.Add(lesson);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Lesson lesson)
    {
        var existing = _lessons.FirstOrDefault(l => l.Id == lesson.Id);
        if (existing == null) return Task.CompletedTask;

        existing.Title = lesson.Title;
        existing.Content = lesson.Content;
        existing.VideoUrl = lesson.VideoUrl;
        existing.Duration = lesson.Duration;
        existing.OrderIndex = lesson.OrderIndex;
        existing.ModuleId = lesson.ModuleId;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int lessonId)
    {
        var lesson = _lessons.FirstOrDefault(l => l.Id == lessonId);
        if (lesson != null)
        {
            _lessons.Remove(lesson);
        }

        return Task.CompletedTask;
    }
}
