using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public interface IEnrollmentService
{
    Task<Enrollment> GetEnrollmentByIdAsync(Guid enrollmentId);
    Task<Enrollment> EnrollAsync(Guid userId, Guid courseId);
}
