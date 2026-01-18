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

    public CourseRepository()
    {
        var course = new Course
        {
            Id = _currentId++,
            Title = "ASP.NET Core",
            Description = "MVC từ A đến Z",
            Modules = new List<CourseModule>()
        };

        var module1 = new CourseModule
        {
            Id = 1,
            Title = "Giới thiệu",
            CourseId = course.Id,
            Lessons = new List<Lesson>()
        };

        module1.Lessons.Add(new Lesson
        {
            Id = 1,
            Title = "MVC là gì?",
            ModuleId = module1.Id
        });

        module1.Lessons.Add(new Lesson
        {
            Id = 2,
            Title = "Cấu trúc project",
            ModuleId = module1.Id
        });

        course.Modules.Add(module1);
        _courses.Add(course);
    }


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
