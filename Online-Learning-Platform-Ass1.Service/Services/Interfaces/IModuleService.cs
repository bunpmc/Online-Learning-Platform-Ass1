using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Service.DTOs.Module;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface IModuleService
{
    Task<IEnumerable<CourseModule>> GetAllAsync();
    Task<CourseModule?> GetByIdAsync(int moduleId);
    Task<IEnumerable<CourseModule>> GetByCourseIdAsync(int courseId);
    Task AddAsync(CourseModule module);
    Task UpdateAsync(CourseModule module);
    Task DeleteAsync(int moduleId);
}
