
public interface ILessonService
{
    Task<IEnumerable<LessonDTO>> GetAllAsync();
    Task<LessonDTO?> GetByIdAsync(Guid lessonId);
    Task<IEnumerable<LessonDTO>> GetByModuleIdAsync(Guid moduleId);

    Task UpdateAsync(LessonDTO lesson);
}
