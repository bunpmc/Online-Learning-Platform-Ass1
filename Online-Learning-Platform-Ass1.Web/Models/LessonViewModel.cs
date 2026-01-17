namespace Online_Learning_Platform_Ass1.Data.Models;

public class LessonViewModel
{
    public int LessonId { get; set; }
    public string Title { get; set; } = string.Empty;

    //để lát hồi highlight bài đang bấm lên thoi
    public bool IsCurrent { get; set; }
}
