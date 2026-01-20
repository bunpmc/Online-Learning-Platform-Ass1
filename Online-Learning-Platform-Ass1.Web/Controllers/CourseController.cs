using Online_Learning_Platform_Ass1.Service.Services.Interfaces;
using Online_Learning_Platform_Ass1.Data.Models;

namespace Online_Learning_Platform_Ass1.Web.Controllers;
public class CourseController(ICourseService courseService, IModuleService moduleService, ILessonService lessonService, IProgressService progressService, IAiLessonService aiLessonService,
    IEnrollmentService enrollmentService) : Controller
{
    private readonly ICourseService _courseService = courseService;
    private readonly IModuleService _moduleService = moduleService;
    private readonly ILessonService _lessonService = lessonService;
    private readonly IAiLessonService _aiLessonService = aiLessonService;
    private readonly IProgressService _progressService = progressService;
    private readonly IEnrollmentService _enrollmentService = enrollmentService;

    // ví dụ như /Course/Learn/5?lessonId=1
    public async Task<IActionResult> Learn(int enrollmentId, int? lessonId)
    {
        var enrollment =
            await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);
        if (enrollment == null) return NotFound();

        var course =
            await _courseService.GetByIdAsync(enrollment.CourseId);
        if (course == null) return NotFound();

        var modules =
            (await _moduleService.GetByCourseIdAsync(course.Id)).ToList();

        var vm = new CourseViewModel
        {
            CourseId = course.Id,
            Title = course.Title,
            Author = course.Author,
            Description = course.Description,
            CurrentLessonId = lessonId
        };

        LessonDTO? currentLesson = null;

        if (lessonId.HasValue)
        {
            currentLesson = await _lessonService.GetByIdAsync(lessonId.Value);
        }

        // bai hoc dau tien
        if (currentLesson == null)
        {
            currentLesson = (await _lessonService
                .GetByModuleIdAsync(modules.First().Id))
                .OrderBy(l => l.OrderIndex)
                .First();
        }

        //Bai học hiện tại
        vm.CurrentLesson = new LessonViewModel
        {
            LessonId = currentLesson.Id,
            Title = currentLesson.Title,
            Content = currentLesson.Content,
            VideoUrl = currentLesson.VideoUrl,
            Duration = currentLesson.Duration,
            OrderIndex = currentLesson.OrderIndex,
            CreatedAt = currentLesson.CreatedAt,
            IsCurrent = true
        };

        foreach (var module in modules)
        {
            var moduleVm = new ModuleViewModel
            {
                ModuleId = module.Id,
                Title = module.Title
            };

            var lessons =
                await _lessonService.GetByModuleIdAsync(module.Id);

            foreach (var lesson in lessons)
            {
                moduleVm.Lessons.Add(new LessonViewModel
                {
                    LessonId = lesson.Id,
                    Title = lesson.Title,
                    IsCurrent = lesson.Id == currentLesson.Id
                });
            }

            vm.Modules.Add(moduleVm);
        }

        return View(vm);
    }


    public async Task<IActionResult> List()
    {
        var courses = await _courseService.GetAllAsync();
        return View(courses);
    }

    [HttpPost]
    public async Task<IActionResult> AiSummary(int enrollmentId, int lessonId)
    {
        var progress = await _progressService.GetLessonProgressAsync(enrollmentId, lessonId);
        if (progress == null) return NotFound();

        if (progress.AiSummaryStatus == Data.Database.Entities.AiSummaryStatus.Processing)
            return Ok(new { status = "processing" });

        var summary = await _aiLessonService.GenerateSummaryAsync(progress);

        return Ok(new
        {
            status = progress.AiSummaryStatus.ToString().ToLower(),
            summary
        });
    }

    [HttpPost]
    public async Task<IActionResult> AiAsk(int enrollmentId, int lessonId, [FromBody] string question)
    {
        var progress = await _progressService.GetLessonProgressAsync(enrollmentId, lessonId);
        var lesson = await _lessonService.GetByIdAsync(lessonId);
        if (progress == null || lesson == null || string.IsNullOrEmpty(lesson.Content))
            return BadRequest();

        var answer = await _aiLessonService.AskAsync(progress, question);
        return Ok(answer);
    }
}
