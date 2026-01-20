using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<IEnumerable<Course>> GetFeaturableCoursesAsync(int count);
    Task<IEnumerable<Course>> GetCoursesAsync(string? searchTerm = null, Guid? categoryId = null);
    Task<int> SaveChangesAsync();
    Task<Course?> GetByIdAsync(int courseId);
    Task AddAsync(Course course);

    Task UpdateAsync(Course course);

    Task DeleteAsync(int courseId);
}
