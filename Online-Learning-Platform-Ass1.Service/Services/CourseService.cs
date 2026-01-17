using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class CourseService (ICourseRepository courseRepository) : ICourseService
{
    private readonly ICourseRepository _courseRepository = courseRepository;
    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _courseRepository.GetAllAsync();
    }

    public async Task<Course?> GetByIdAsync(int courseId)
    {
        return await _courseRepository.GetByIdAsync(courseId);
    }

    public async Task AddAsync(Course course)
    {
        await _courseRepository.AddAsync(course);
    }

    public async Task UpdateAsync(Course course)
    {
        await _courseRepository.UpdateAsync(course);
    }

    public async Task DeleteAsync(int courseId)
    {
        await _courseRepository.DeleteAsync(courseId);
    }
}
