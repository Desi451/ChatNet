using ChatNet.Domain.Entity;

namespace ChatNet.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUser(int id);
    Task<List<User>> GetAviableFriends();
    Task<List<User>> GetUsersWhoInvited(int invitedUserId);
    Task<List<int>> GetInvitedFriendsByUserId(int userId);
    Task AddInvite(int userId, int invitedId);
    Task RemoveInvite(int userId, int invitedId);
    Task AddFriend(int userId, int friendId);
}
