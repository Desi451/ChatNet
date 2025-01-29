using Microsoft.AspNetCore.SignalR;

namespace ChatNet.SignalR;

public class NotficationsHub : Hub
{
    private readonly OnlineUsersService _onlineUsersService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotficationsHub(OnlineUsersService onlineUsersService, IHttpContextAccessor httpContextAccessor)
    {
        _onlineUsersService = onlineUsersService;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task OnConnectedAsync()
    {
        var userName = _httpContextAccessor.HttpContext.Session
            .GetString("UserName") ?? "Unknown";

        await Groups.AddToGroupAsync(Context.ConnectionId, userName);
        _onlineUsersService.AddUserToGroup(Context.ConnectionId, userName);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userName = _httpContextAccessor.HttpContext.Session
            .GetString("UserName") ?? "Unknown";

        _onlineUsersService.RemoveUserFromGroup(Context.ConnectionId, userName);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string username, string message)
    {
        await Clients.Group(username).SendAsync("ReceiveNotification", message);
    }
}