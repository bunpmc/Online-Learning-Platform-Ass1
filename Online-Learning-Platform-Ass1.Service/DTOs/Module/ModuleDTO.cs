using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Learning_Platform_Ass1.Service.DTOs.Module;

public class ModuleDTO
{

    public Guid Id { get; set; }

    public Guid CourseId { get; set; }


    public string Title { get; set; } = null!;

    public int OrderIndex { get; set; }
}
