using ChatNet.Domain.Entity;

namespace ChatNet.Models;

public class ChatViewModel
{
    public ChatViewModel(List<User> friends)
    {
        Friends = friends;
    }

    public ChatViewModel(List<User> friends, List<string> onlineUsers, string userName)
    {
        Friends = friends;
        OnlineUsers = onlineUsers;
        UserName = userName;
    }

    public User CurrentChat { get; set; }
    public string UserName { get; set; }
    public List<string> OnlineUsers { get; set; }
    public List<User> Friends { get; set; } = [];
    public List<int> ChatGroups { get; set; } = [];

}
