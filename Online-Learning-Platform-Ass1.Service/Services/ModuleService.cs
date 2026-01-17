using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Module;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class ModuleService(IModuleRepository moduleRepository) : IModuleService
{
    private readonly IModuleRepository _moduleRepository = moduleRepository;
    public async Task<IEnumerable<CourseModule>> GetAllAsync()
    {
        return await _moduleRepository.GetAllAsync();
    }

    public async Task<CourseModule?> GetByIdAsync(int moduleId)
    {
        return await _moduleRepository.GetByIdAsync(moduleId);
    }

    public async Task<IEnumerable<CourseModule>> GetByCourseIdAsync(int courseId)
    {
        return await _moduleRepository.GetByCourseIdAsync(courseId);
    }

    public async Task AddAsync(CourseModule module)
    {
        await _moduleRepository.AddAsync(module);
    }

    public async Task UpdateAsync(CourseModule module)
    {
        await _moduleRepository.UpdateAsync(module);
    }

    public async Task DeleteAsync(int moduleId)
    {
        await _moduleRepository.DeleteAsync(moduleId);
    }
}
