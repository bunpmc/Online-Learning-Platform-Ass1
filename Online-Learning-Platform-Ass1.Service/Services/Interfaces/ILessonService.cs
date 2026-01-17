using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface ILessonService
{
    Task<IEnumerable<Lesson>> GetAllAsync();
    Task<Lesson?> GetByIdAsync(int lessonId);
    Task<IEnumerable<Lesson>> GetByModuleIdAsync(int moduleId);
    Task AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(int lessonId);
}
