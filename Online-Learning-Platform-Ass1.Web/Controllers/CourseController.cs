using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Online_Learning_Platform_Ass1.Data.Models;
using Online_Learning_Platform_Ass1.Service.DTOs.Course;
using Online_Learning_Platform_Ass1.Service.DTOs.Order;
using Online_Learning_Platform_Ass1.Service.Services;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

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

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = null;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var parsedId))
        {
            userId = parsedId;
        }

        var course = await courseService.GetCourseDetailsAsync(id, userId);
        if (course == null) return NotFound();
        return View(course);
    }

    // GET: Confirm Purchase
    public async Task<IActionResult> Checkout(Guid id)
    {
        var course = await courseService.GetCourseDetailsAsync(id);
        if (course == null) return NotFound();

        // Check if user is logged in (Authorize attribute handles it but double check logic if needed)
        // Check if already enrolled (Service logic usually, but here we just show checkout)

        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var order = await orderService.CreateOrderAsync(userId, new CreateOrderDto { CourseId = id });
        if (order == null) return BadRequest("Could not create order.");

        // Redirect to VNPay
        return RedirectToAction("CreatePaymentUrl", "Payment", new { orderId = order.OrderId });
    }

    public IActionResult Success(Guid id)
    {
        ViewBag.OrderId = id;
        return View();
    }
    public async Task<IActionResult> MyCourses()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return RedirectToAction("Login", "User");
        }

        var courses = await courseService.GetEnrolledCoursesAsync(userId);
        return View(courses);
    }

    // ví dụ như /Course/Learn/5?lessonId=1
    public async Task<IActionResult> Learn(Guid enrollmentId, Guid? lessonId)
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
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price,
            ImageUrl = course.ImageUrl,
            InstructorName = course.InstructorId,
            CategoryName = course.CategoryId.ToString(),
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
            Id = currentLesson.Id,
            Title = currentLesson.Title,
            VideoUrl = currentLesson.ContentUrl

        };

        foreach (var module in modules)
        {
            var moduleVm = new ModuleViewModel
            {
                Id = module.Id,
                Lessons = new List<LessonViewModel>()
            };

            var lessons =
                await _lessonService.GetByModuleIdAsync(module.Id);

            foreach (var lesson in lessons)
            {
                moduleVm.Lessons.Add(new LessonViewModel
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    VideoUrl = lesson.ContentUrl
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
    public async Task<IActionResult> AiSummary(Guid enrollmentId, Guid? lessonId)
    {
        var progress = await _progressService.GetLessonProgressAsync(enrollmentId, lessonId);
        if (progress == null) return NotFound();

        if (progress.AiSummaryStatus.Equals(AiSummaryStatus.Pending))
            return Ok(new { status = "processing" });

        var summary = await _aiLessonService.GenerateSummaryAsync(progress);

        return Ok(new
        {
            status = progress.AiSummaryStatus.ToString().ToLower(),
            summary
        });
    }

    [HttpPost]
    public async Task<IActionResult> AiAsk(Guid enrollmentId, Guid lessonId, [FromBody] string question)
    {
        var progress = await _progressService.GetLessonProgressAsync(enrollmentId, lessonId);
        var lesson = await _lessonService.GetByIdAsync(lessonId);
        if (progress == null || lesson == null || string.IsNullOrEmpty(lesson.ContentUrl))
            return BadRequest();

        var answer = await _aiLessonService.AskAsync(progress, question);
        return Ok(answer);
    }
}
