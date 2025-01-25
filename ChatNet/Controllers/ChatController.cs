using ChatNet.Models;
using ChatNet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers;
public class ChatController : Controller
{
    private readonly IUserService _userService;

    public ChatController(IUserService userService)
    {
        _userService = userService;
    }


    public async Task<IActionResult> Index()
    {
        var id = (int)HttpContext.Session.GetInt32("id");

        var user = await _userService.GetUser(id);

        var model = new ChatViewModel(user.Friends);

        return View(model);
    }
}
