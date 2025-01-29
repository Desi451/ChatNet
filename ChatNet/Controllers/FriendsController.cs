using ChatNet.Domain.Entity;
using ChatNet.Models;
using ChatNet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatNet.Controllers;

public class FriendsController : Controller
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;

    public FriendsController(IUserService userService, IChatService chatService)
    {
        _userService = userService;
        _chatService = chatService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var id = (int)HttpContext.Session.GetInt32("id");

        var availableFriends = await _userService.GetAviableFriends();
        var friendsIds = await _userService.GetInvitedFriendsByUserId(id);
        var user = await _userService.GetUser(id);

        var data = new SearchFriendViewModel
        {
            AviableFriends = new Dictionary<User, bool>(),
            UserInvites = await _userService.GetUsersWhoInvited(id),
            Friends = user.Friends
        };

        foreach (var friend in availableFriends.ToList())
        {

            if (data.UserInvites.Any(u => u.Id == friend.Id))
            {
                availableFriends.Remove(friend);
                continue;
            }

            bool isFriend = friendsIds.Contains(friend.Id);

            data.AviableFriends[friend] = isFriend;
        }

        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> AddInvite(int invitedId, bool isUserInvite)
    {
        var id = (int)HttpContext.Session.GetInt32("id");
        var invitedUser = await _userService.GetUser(invitedId);

        if (isUserInvite)
        {
            await _userService.AddInvite(id, invitedId);
        }
        else
        {
            await _userService.AddFriend(id, invitedId);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveInvite(int invitedId, bool isUserInvite)
    {
        var id = (int)HttpContext.Session.GetInt32("id");
        var invitedUser = await _userService.GetUser(invitedId);

        if (isUserInvite)
        {
            await _userService.RemoveInvite(id, invitedId);
        }
        else
        {
            await _userService.RemoveInvite(invitedId, id);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFriend(int friendId)
    {
        var id = (int)HttpContext.Session.GetInt32("id");
        await _userService.RemoveFriend(id, friendId);
        await _chatService.RemoveMessagesFromChat(id, friendId);

        return RedirectToAction("Index");
    }
}
