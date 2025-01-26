using Microsoft.AspNetCore.SignalR;

namespace ChatNet.SignalR;

public class ChatHub : Hub
{
    private readonly OnlineUsersService _onlineUsersService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatHub(OnlineUsersService onlineUsersService, IHttpContextAccessor httpContextAccessor)
    {
        _onlineUsersService = onlineUsersService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SendMessage(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinChat(string groupName)
    {
        var userName = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? "Unknown";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _onlineUsersService.AddUserToGroup(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserJoined", userName);
    }

    public async Task LeaveChat(string groupName)
    {
        var userName = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? "Unknown";

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _onlineUsersService.RemoveUserFromGroup(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserLeft", userName);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userGroups = _onlineUsersService.GetUserGroups(Context.ConnectionId);
        foreach (var groupName in userGroups)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
