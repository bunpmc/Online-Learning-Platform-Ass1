using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface IProgressService
{
    Task<LessonProgress?> GetLessonProgressAsync(int enrollmentId, int lessonId);
    Task<IEnumerable<LessonProgress>> GetCourseProgressAsync(int enrollmentId);
    Task UpdateWatchedPositionAsync(int enrollmentId, int lessonId, int watchedPosition);
    Task CompleteLessonAsync(int enrollmentId, int lessonId );
    Task<double> GetCourseCompletionPercentageAsync(int enrollmentId);
}
