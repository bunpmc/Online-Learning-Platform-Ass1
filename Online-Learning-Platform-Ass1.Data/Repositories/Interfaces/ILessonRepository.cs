using Online_Learning_Platform_Ass1.Data.Database.Entities;

public interface ILessonRepository
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(Guid lessonId);
    Task<IEnumerable<Lesson>> GetByModuleIdAsync(Guid moduleId);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Guid lessonId);
}
