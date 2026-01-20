using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public class CourseService(
    ICourseRepository courseRepo,
    IEnrollmentRepository enrollmentRepo
) : ICourseService
{    public async Task<IEnumerable<CourseDTO>> GetAllAsync()
        => (await courseRepo.GetAllAsync())
            .Select(c => new CourseDTO
            {
                Id = c.Id,
                InstructorId = c.InstructorId,
                CategoryId = c.CategoryId,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            });

    public async Task<CourseDTO?> GetByIdAsync(Guid id)
    {
        var c = await courseRepo.GetByIdAsync(id);
        return c == null ? null : new CourseDTO
        {
            Id = c.Id,
            InstructorId = c.InstructorId,
            CategoryId = c.CategoryId,
            Title = c.Title,
            Description = c.Description,
            ImageUrl = c.ImageUrl,
            Price = c.Price,
            Status = c.Status,
            CreatedAt = c.CreatedAt
        };
    }

    public async Task<IEnumerable<CourseViewModel>> GetFeaturedCoursesAsync()
        => (await courseRepo.GetFeaturableCoursesAsync(6))
            .Select(c => MapCourseVM(c));

    public async Task<IEnumerable<CourseViewModel>> GetAllCoursesAsync(string? s, Guid? cat)
        => (await courseRepo.GetCoursesAsync(s, cat))
            .Select(c => MapCourseVM(c));

    public async Task<CourseDetailViewModel?> GetCourseDetailsAsync(Guid id, Guid? userId)
    {
        var c = await courseRepo.GetByIdAsync(id);
        if (c == null) return null;

        return new CourseDetailViewModel
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Price = c.Price,
            ImageUrl = c.ImageUrl,
            InstructorName = c.Instructor.Username,
            CategoryName = c.Category.Name,
            IsEnrolled = userId.HasValue &&
                await enrollmentRepo.IsEnrolledAsync(userId.Value, id),
            Modules = c.Modules.Select(m => new ModuleViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Lessons = m.Lessons.Select(l => new LessonViewModel
                {
                    Id = l.Id,
                    Title = l.Title,
                    VideoUrl = l.ContentUrl
                })
            })
        };
    }

    public async Task<IEnumerable<CourseViewModel>> GetEnrolledCoursesAsync(Guid userId)
        => (await enrollmentRepo.GetStudentEnrollmentsAsync(userId))
            .Select(e => MapCourseVM(e.Course));

    private static CourseViewModel MapCourseVM(Course c)
        => new()
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Price = c.Price,
            ImageUrl = c.ImageUrl,
            InstructorName = c.Instructor.Username,
            CategoryName = c.Category.Name
        };
}
