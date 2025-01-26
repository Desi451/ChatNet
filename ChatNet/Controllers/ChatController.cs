using ChatNet.Models;
using ChatNet.Services.Interfaces;
using ChatNet.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers;
public class ChatController : Controller
{
    private readonly IUserService _userService;
    private readonly OnlineUsersService _onlineUsersService;

    public ChatController(IUserService userService, OnlineUsersService onlineUsersService)
    {
        _userService = userService;
        _onlineUsersService = onlineUsersService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await LoadData();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadChat(int userId)
    {
        var model = await LoadData();
        model.CurrentChat = await _userService.GetUser(userId);
        return PartialView("ChatRoomPartial", model);
    }

    public async Task<ChatViewModel> LoadData()
    {
        var id = (int)HttpContext.Session.GetInt32("id");

        var user = await _userService.GetUser(id);
        var onlineUsers = _onlineUsersService.GetOnlineUsers();

        return new ChatViewModel(user.Friends, onlineUsers.ToList(), user.Name);
    }
}
