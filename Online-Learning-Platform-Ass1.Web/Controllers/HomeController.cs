using System.Diagnostics;
using Online_Learning_Platform_Ass1.Data.Models;

namespace Online_Learning_Platform_Ass1.Web.Controllers;
public class HomeController : Controller
{
    public IActionResult IndexAsync() => View();

    public IActionResult PrivacyAsync() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ErrorAsync() => View(new ErrorViewModel
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
