using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.Category;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDTO>> GetAllAsync();
    Task<CourseDTO?> GetByIdAsync(Guid id);

    Task<IEnumerable<CourseViewModel>> GetFeaturedCoursesAsync();
    Task<IEnumerable<CourseViewModel>> GetAllCoursesAsync(string? searchTerm, Guid? categoryId);
    Task<CourseDetailViewModel?> GetCourseDetailsAsync(Guid id, Guid? userId);
    Task<IEnumerable<CourseViewModel>> GetEnrolledCoursesAsync(Guid userId);
}
