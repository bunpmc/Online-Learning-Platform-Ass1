using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByIdAsync(int enrollmentId);
    Task<Enrollment?> GetByUserAndCourseAsync(int userId, int courseId);
    Task<IEnumerable<Enrollment>> GetByUserIdAsync(int userId);

    Task AddAsync(Enrollment enrollment);
}
