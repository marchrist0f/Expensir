using Microsoft.AspNetCore.Mvc;
using Expensir.Data;

namespace ExpensesTracker.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string Username, string Password)
    {
        var user = _context.Users.FirstOrDefault(
            u => u.Username == Username &&
                 u.Password == Password
        );

        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Dashboard");
        }

        ViewBag.Error = "Invalid username or password";

        return View();
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Login");
        }

        return View();
    }
}