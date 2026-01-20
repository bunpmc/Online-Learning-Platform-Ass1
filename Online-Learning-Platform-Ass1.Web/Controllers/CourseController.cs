using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Service.DTOs.Order;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;
using Online_Learning_Platform_Ass1.Data.Models;

namespace Online_Learning_Platform_Ass1.Web.Controllers;
namespace Online_Learning_Platform_Ass1.Data.Controllers;

public class CourseController(ICourseService courseService, IModuleService moduleService, ILessonService lessonService, IProgressService progressService, IAiLessonService aiLessonService,
    IEnrollmentService enrollmentService) : Controller
{
    private readonly ICourseService _courseService = courseService;
    private readonly IModuleService _moduleService = moduleService;
    private readonly ILessonService _lessonService = lessonService;
    private readonly IAiLessonService _aiLessonService = aiLessonService;
    private readonly IProgressService _progressService = progressService;
    private readonly IEnrollmentService _enrollmentService = enrollmentService;


[Authorize]
public class CourseController(
    ICourseService courseService,
    IOrderService orderService) : Controller
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
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
            CourseId = course.Id,
            Title = course.Title,
            Author = course.Author,
            Description = course.Description,
            CurrentLessonId = lessonId
        };

        LessonDTO? currentLesson = null;

        if (lessonId.HasValue)
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = null;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var parsedId))
        {
            userId = parsedId;
            currentLesson = await _lessonService.GetByIdAsync(lessonId.Value);
        }

        var course = await courseService.GetCourseDetailsAsync(id, userId);
        if (course == null) return NotFound();
        return View(course);
        // bai hoc dau tien
        if (currentLesson == null)
        {
            currentLesson = (await _lessonService
                .GetByModuleIdAsync(modules.First().Id))
                .OrderBy(l => l.OrderIndex)
                .First();
    }

    // GET: Confirm Purchase
    public async Task<IActionResult> Checkout(Guid id)
        //Bai học hiện tại
        vm.CurrentLesson = new LessonViewModel
    {
        var course = await courseService.GetCourseDetailsAsync(id);
        if (course == null) return NotFound();
            LessonId = currentLesson.Id,
            Title = currentLesson.Title,
            Content = currentLesson.Content,
            VideoUrl = currentLesson.VideoUrl,
            Duration = currentLesson.Duration,
            OrderIndex = currentLesson.OrderIndex,
            CreatedAt = currentLesson.CreatedAt,
            IsCurrent = true
        };

        // Check if user is logged in (Authorize attribute handles it but double check logic if needed)
        // Check if already enrolled (Service logic usually, but here we just show checkout)
        foreach (var module in modules)
        {
            var moduleVm = new ModuleViewModel
            {
                ModuleId = module.Id,
                Title = module.Title
            };

        return View(course);
    }
            var lessons =
                await _lessonService.GetByModuleIdAsync(module.Id);

    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(Guid id)
            foreach (var lesson in lessons)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                moduleVm.Lessons.Add(new LessonViewModel
        {
            return Unauthorized();
                    LessonId = lesson.Id,
                    Title = lesson.Title,
                    IsCurrent = lesson.Id == currentLesson.Id
                });
        }

        var order = await orderService.CreateOrderAsync(userId, new CreateOrderDto { CourseId = id });
        if (order == null) return BadRequest("Could not create order.");
            vm.Modules.Add(moduleVm);
        }

        // Redirect to VNPay
        return RedirectToAction("CreatePaymentUrl", "Payment", new { orderId = order.OrderId });
        return View(vm);
    }

    public IActionResult Success(Guid id)

    public async Task<IActionResult> List()
    {
        ViewBag.OrderId = id;
        return View();
        var courses = await _courseService.GetAllAsync();
        return View(courses);
    }
    public async Task<IActionResult> MyCourses()

    [HttpPost]
    public async Task<IActionResult> AiSummary(int enrollmentId, int lessonId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        var progress = await _progressService.GetLessonProgressAsync(enrollmentId, lessonId);
        if (progress == null) return NotFound();

        if (progress.AiSummaryStatus == Data.Database.Entities.AiSummaryStatus.Processing)
            return Ok(new { status = "processing" });

        var summary = await _aiLessonService.GenerateSummaryAsync(progress);

        return Ok(new
        {
            return RedirectToAction("Login", "User");
            status = progress.AiSummaryStatus.ToString().ToLower(),
            summary
        });
        }

        var courses = await courseService.GetEnrolledCoursesAsync(userId);
        return View(courses);
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
