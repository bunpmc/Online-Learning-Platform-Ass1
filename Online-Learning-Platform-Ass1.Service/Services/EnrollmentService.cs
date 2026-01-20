using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class EnrollmentService(IEnrollmentRepository enrollmentRepository) : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository = enrollmentRepository;

    public async Task<Enrollment> GetEnrollmentByIdAsync(int enrollmentId)
    {
        return await _enrollmentRepository.GetByIdAsync(enrollmentId)
            ?? throw new Exception("Enrollment not found");
    }

    public async Task<Enrollment> EnrollAsync(int userId, int courseId)
    {
        var existing =
            await _enrollmentRepository.GetByUserAndCourseAsync(userId, courseId);

        if (existing != null)
            return existing;

        var enrollment = new Enrollment
        {
            UserId = userId,
            CourseId = courseId
        };

        await _enrollmentRepository.AddAsync(enrollment);
        return enrollment;
    }
}

