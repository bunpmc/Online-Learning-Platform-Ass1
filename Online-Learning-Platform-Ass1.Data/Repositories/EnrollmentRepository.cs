using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly List<Enrollment> _enrollments = new();

    public Task<Enrollment?> GetByIdAsync(Guid enrollmentId)
    {
        return Task.FromResult(
            _enrollments.FirstOrDefault(e => e.Id == enrollmentId)
        );
    }

    public Task<Enrollment?> GetByUserAndCourseAsync(Guid userId, Guid courseId)
    {
        return Task.FromResult(
            _enrollments.FirstOrDefault(e =>
                e.UserId == userId &&
                e.CourseId == courseId)
        );
    }

    public Task<IEnumerable<Enrollment>> GetByUserIdAsync(Guid userId)
    {
        return Task.FromResult(
            _enrollments.Where(e => e.UserId == userId).AsEnumerable()
        );
    }

    public Task<IEnumerable<Enrollment>> GetStudentEnrollmentsAsync(Guid userId)
        => GetByUserIdAsync(userId);

    public Task<bool> IsEnrolledAsync(Guid userId, Guid courseId)
    {
        return Task.FromResult(
            _enrollments.Any(e =>
                e.UserId == userId &&
                e.CourseId == courseId &&
                e.Status == "active")
        );
    }

    public Task AddAsync(Enrollment enrollment)
    {
        enrollment.Id = Guid.NewGuid();
        enrollment.EnrolledAt = DateTime.UtcNow;
        _enrollments.Add(enrollment);
        return Task.CompletedTask;
    }
}
