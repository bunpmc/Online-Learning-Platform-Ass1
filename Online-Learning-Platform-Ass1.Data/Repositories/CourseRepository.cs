using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class CourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = new();
    private int _currentId = 1;

    public Task<IEnumerable<Course>> GetAllAsync()
    {
        return Task.FromResult(_courses.AsEnumerable());
    }

    public Task<Course?> GetByIdAsync(int courseId)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        return Task.FromResult(course);
    }

    public Task AddAsync(Course course)
    {
        course.Id = _currentId++;
        _courses.Add(course);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Course course)
    {
        var existing = _courses.FirstOrDefault(c => c.Id == course.Id);
        if (existing == null) return Task.CompletedTask;

        existing.Title = course.Title;
        existing.Description = course.Description;
        existing.Modules = course.Modules;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int courseId)
    {
        var course = _courses.FirstOrDefault(c => c.Id == courseId);
        if (course != null)
        {
            _courses.Remove(course);
        }
        return Task.CompletedTask;
    }
}
