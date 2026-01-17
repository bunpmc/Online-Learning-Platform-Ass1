using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
public interface IProgressRepository
{
    Task<LessonProgress?> GetByIdAsync(int progressId);
    Task<LessonProgress?> GetByEnrollmentAndLessonAsync(int enrollmentId, int lessonId);
    Task<IEnumerable<LessonProgress>> GetByEnrollmentIdAsync(int enrollmentId);
    Task AddAsync(LessonProgress progress);
    Task UpdateAsync(LessonProgress progress);
    Task MarkCompletedAsync(int enrollmentId, int lessonId);
    Task UpdateWatchedPositionAsync(int enrollmentId, int lessonId, int lastWatchedPosition);
    Task DeleteAsync(int progressId);

}
