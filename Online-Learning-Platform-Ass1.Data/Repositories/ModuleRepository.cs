using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class ModuleRepository : IModuleRepository
{
    private readonly List<CourseModule> _modules = new();
    private int _currentId = 1;

    public ModuleRepository()
    {
        _modules.Add(new CourseModule
        {
            Id = _currentId++,
            Title = "Giới thiệu ASP.NET Core",
            OrderIndex = 1,
            CourseId = 1
        });

        _modules.Add(new CourseModule
        {
            Id = _currentId++,
            Title = "MVC cơ bản",
            OrderIndex = 2,
            CourseId = 1
        });

        _modules.Add(new CourseModule
        {
            Id = _currentId++,
            Title = "Entity Framework Core",
            OrderIndex = 1,
            CourseId = 2
        });
    }

    public Task<IEnumerable<CourseModule>> GetAllAsync()
    {
        return Task.FromResult(_modules.AsEnumerable());
    }

    public Task<CourseModule?> GetByIdAsync(int moduleId)
    {
        var module = _modules.FirstOrDefault(m => m.Id == moduleId);
        return Task.FromResult(module);
    }

    public Task<IEnumerable<CourseModule>> GetByCourseIdAsync(Guid courseId)
    {
        var modules = _modules
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.OrderIndex)
            .AsEnumerable();

        return Task.FromResult(modules);
    }

    public Task AddAsync(CourseModule module)
    {
        module.Id = _currentId++;
        _modules.Add(module);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CourseModule module)
    {
        var existing = _modules.FirstOrDefault(m => m.Id == module.Id);
        if (existing == null) return Task.CompletedTask;

        existing.Title = module.Title;
        existing.OrderIndex = module.OrderIndex;
        existing.CourseId = module.CourseId;
        existing.Lessons = module.Lessons;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid moduleId)
    {
        var module = _modules.FirstOrDefault(m => m.Id == moduleId);
        if (module != null)
        {
            _modules.Remove(module);
        }
        return Task.CompletedTask;
    }
}
