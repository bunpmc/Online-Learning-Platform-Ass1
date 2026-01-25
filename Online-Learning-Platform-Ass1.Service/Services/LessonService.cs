using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class LessonService(ILessonRepository lessonRepository) : ILessonService
{
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    public async Task<IEnumerable<LessonDTO>> GetAllAsync()
    {
        var lessons = await _lessonRepository.GetAllAsync();

        return lessons.Select(l => new LessonDTO
        {
            Id = l.Id,
            ModuleId = l.ModuleId,
            Title = l.Title,
            Type = l.Type,
            ContentUrl = l.ContentUrl,
            Duration = l.Duration,
            OrderIndex = l.OrderIndex
        });
    }

    public async Task<LessonDTO?> GetByIdAsync(Guid lessonId)
    {
        var l = await _lessonRepository.GetByIdAsync(lessonId);
        if (l == null) return null;

        return new LessonDTO
        {
            Id = l.Id,
            ModuleId = l.ModuleId,
            Title = l.Title,
            Type = l.Type,
            ContentUrl = l.ContentUrl,
            Duration = l.Duration,
            OrderIndex = l.OrderIndex
        };
    }

    public async Task<IEnumerable<LessonDTO>> GetByModuleIdAsync(Guid moduleId)
    {
        var lessons = await _lessonRepository.GetByModuleIdAsync(moduleId);

        return lessons.Select(l => new LessonDTO
        {
            Id = l.Id,
            ModuleId = l.ModuleId,
            Title = l.Title,
            Type = l.Type,
            ContentUrl = l.ContentUrl,
            Duration = l.Duration,
            OrderIndex = l.OrderIndex
        });
    }

    public async Task UpdateAsync(LessonDTO dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(dto.Id);
        if (lesson == null)
            throw new Exception("Lesson not found");

        lesson.Title = dto.Title;
        lesson.Type = dto.Type;
        lesson.ContentUrl = dto.ContentUrl;
        lesson.Duration = dto.Duration;
        lesson.OrderIndex = dto.OrderIndex;

        await _lessonRepository.UpdateAsync(lesson);
    }
}
