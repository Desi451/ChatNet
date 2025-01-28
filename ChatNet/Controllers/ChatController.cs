using ChatNet.Domain.Entity;
using ChatNet.Models;
using ChatNet.Services.Interfaces;
using ChatNet.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers;
public class ChatController : Controller
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    private readonly OnlineUsersService _onlineUsersService;

    public ChatController(IUserService userService, IChatService chatService, OnlineUsersService onlineUsersService)
    {
        _userService = userService;
        _onlineUsersService = onlineUsersService;
        _chatService = chatService;
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

    [HttpPost]
    public async Task SaveMessage(int reciverId, string message)
    {
        var id = (int)HttpContext.Session.GetInt32("id");

        var user = await _userService.GetUser(id);
        var reciver = await _userService.GetUser(reciverId);

        var result = new Message(user, reciver,message);
        await _chatService.AddMessage(result);
    }

    public async Task<ChatViewModel> LoadData()
    {
        var id = (int)HttpContext.Session.GetInt32("id");

        var user = await _userService.GetUser(id);
        var onlineUsers = _onlineUsersService.GetOnlineUsers();

        return new ChatViewModel(user.Friends, onlineUsers.ToList(), user.Name);
    }
}
