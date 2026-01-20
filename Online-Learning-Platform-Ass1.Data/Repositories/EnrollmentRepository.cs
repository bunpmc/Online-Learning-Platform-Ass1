using Microsoft.EntityFrameworkCore;
using Online_Learning_Platform_Ass1.Data.Database;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;
public class EnrollmentRepository() : IEnrollmentRepository
{

{
    {
            .Where(e => e.UserId == userId)
    }

    {
    }
}
