public class LessonDTO
{

    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public int Duration { get; set; }

    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
}
