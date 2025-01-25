using ChatNet.Domain.Entity;

namespace ChatNet.Models;

public class SearchFriendViewModel
{
    public Dictionary<User, bool> AviableFriends { get; set; } = [];
    public List<User> UserInvites { get; set; } = [];
}
