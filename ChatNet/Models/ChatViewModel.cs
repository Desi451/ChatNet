using ChatNet.Domain.Entity;

namespace ChatNet.Models;

public class ChatViewModel
{
    public ChatViewModel(List<User> friends)
    {
        Friends = friends;
    }

    public List<User> Friends { get; set; } = [];
    public List<int> ChatGroups { get; set; } = [];

}
