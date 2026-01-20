using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class EnrollmentRepository() : IEnrollmentRepository
{

    private readonly List<Enrollment> _enrollments = new();
    private int _currentId = 1;

    public Task<Enrollment?> GetByIdAsync(int enrollmentId)
    {
        var result = _enrollments.FirstOrDefault(e => e.Id == enrollmentId);
        return Task.FromResult(result);
    }

    public Task<Enrollment?> GetByUserAndCourseAsync(int userId, int courseId)
    {
        var result = _enrollments.FirstOrDefault(e =>
            e.UserId == userId &&
            e.CourseId == courseId);

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Enrollment>> GetByUserIdAsync(int userId)
    {
        var result = _enrollments
            .Where(e => e.UserId == userId)
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task AddAsync(Enrollment enrollment)
    {
        enrollment.Id = _currentId++;
        enrollment.EnrolledAt = DateTime.UtcNow;

        _enrollments.Add(enrollment);
        return Task.CompletedTask;
    }
}
