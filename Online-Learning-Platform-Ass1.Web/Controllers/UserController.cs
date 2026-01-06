using Online_Learning_Platform_Ass1.Service.DTOs.User;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Controllers;

public class UserController(IUserService userService) : Controller
{
    // GET: User/Login
    [HttpGet]
    public IActionResult LoginAsync() => View();

    // POST: User/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid) return View(userLoginDto);

        var result = await userService.LoginAsync(userLoginDto);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Login successful!";
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Login failed");
        return View(userLoginDto);
    }

    // GET: User/Register
    [HttpGet]
    public IActionResult RegisterAsync() => View();

    // POST: User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid) return View(userRegisterDto);

        var result = await userService.RegisterAsync(userRegisterDto);

        if (result.Success)
        {
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction(nameof(LoginAsync));
        }

        ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed");
        return View(userRegisterDto);
    }

    // GET: User/Logout
    [HttpGet]
    public IActionResult LogoutAsync()
    {
        TempData["SuccessMessage"] = "You have been logged out";
        return RedirectToAction("Index", "Home");
    }
}
