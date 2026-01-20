using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class EnrollmentService(IEnrollmentRepository enrollmentRepository)
    : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository = enrollmentRepository;

    public async Task<Enrollment> GetEnrollmentByIdAsync(Guid enrollmentId)
    {
        return await _enrollmentRepository.GetByIdAsync(enrollmentId)
            ?? throw new Exception("Enrollment not found");
    }

    public async Task<Enrollment> EnrollAsync(Guid userId, Guid courseId)
    {
        var existing =
            await _enrollmentRepository.IsEnrolledAsync(userId, courseId);

        if (existing)
            throw new Exception("User already enrolled in this course");

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseId = courseId,
            Status = "active",
            EnrolledAt = DateTime.UtcNow
        };

        await _enrollmentRepository.AddAsync(enrollment);
        return enrollment;
    }
}
