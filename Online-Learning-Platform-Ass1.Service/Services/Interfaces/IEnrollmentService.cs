using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Learning_Platform_Ass1.Data.Database.Entities;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface IEnrollmentService
{
    Task<Enrollment> GetEnrollmentByIdAsync(int enrollmentId);
    Task<Enrollment> EnrollAsync(int userId, int courseId);
}
