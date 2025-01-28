using ChatNet.Database;
using ChatNet.Domain.Entity;
using ChatNet.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatNet.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("", "All fields are required.");
            return View();
        }

        var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
        if (existingUser != null)
        {
            ModelState.AddModelError("", "A user with this email already exists.");
            return View();
        }

        var newUser = new User(username, password, email);
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Both fields are required.");
            return View();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            ModelState.AddModelError("", "No user found");
            return View();
        }

        var result = PasswordHelper.VerifyPassword(password, user.Password);

        if (!result)
        {
            ModelState.AddModelError("", "Bad password");
            return View();
        }

        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetInt32("id", user.Id);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
