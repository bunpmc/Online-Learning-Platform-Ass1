using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
public interface IModuleRepository
{
    Task<IEnumerable<CourseModule>> GetAllAsync();
    Task<CourseModule?> GetByIdAsync(Guid moduleId);
    Task<IEnumerable<CourseModule>> GetByCourseIdAsync(Guid courseId);
    Task AddAsync(CourseModule module);
    Task UpdateAsync(CourseModule module);
    Task DeleteAsync(Guid moduleId);
}
