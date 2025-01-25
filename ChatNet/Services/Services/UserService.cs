using ChatNet.Database;
using ChatNet.Domain.Entity;
using ChatNet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatNet.Services.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> GetUser(int id)
    {
        return await _context.Users.Where(u => u.Id == id)
            .Include(u => u.InvitedFriends)
            .Include(u => u.Friends)
            .FirstAsync();
    }

    public async Task<List<User>> GetAviableFriends()
    {
        var userId = _httpContextAccessor.HttpContext.Session.GetInt32("id");

        if (userId == null)
        {
            return new List<User>();
        }

        var currentUser = await GetUser(userId.Value);

        if (currentUser == null)
        {
            return new List<User>();
        }

        var availableFriends = await _context.Users
            .Where(u => u.AllowInvites == true && u.Id != userId)
            .ToListAsync();


        foreach (var friend in availableFriends.ToList())
        {

            if (currentUser.Friends.Any(u => u.Id == friend.Id))
            {
                availableFriends.Remove(friend);
            }
        }

        return availableFriends;
    }

    public async Task<List<int>> GetInvitedFriendsByUserId(int userId)
    {
        var user = await GetUser(userId);

        return user?.InvitedFriends.Select(f => f.Id).ToList() ?? [];
    }

    public async Task AddInvite(int userId, int invitedId)
    {
        var user = await GetUser(userId);
        var invitedUser = await GetUser(invitedId);

        user.InvitedFriends.Add(invitedUser);
        await _context.SaveChangesAsync();
    }


    public async Task RemoveInvite(int userId, int invitedId)
    {
        var user = await GetUser(userId);
        var invitedUser = await GetUser(invitedId);

        user.InvitedFriends.Remove(invitedUser);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersWhoInvited(int invitedUserId)
    {
        var usersWhoInvited = await _context.Users
            .Where(u => _context.Set<Dictionary<string, object>>("UserInvitedFriends")
                .Any(invite => (int)invite["UserId"] == u.Id && (int)invite["InvitedFriendId"] == invitedUserId))
            .ToListAsync();

        return usersWhoInvited;
    }

    public async Task AddFriend(int userId, int friendId)
    {
        var user = await GetUser(userId);
        var friendUser = await GetUser(friendId);

        user.Friends.Add(friendUser);
        friendUser.Friends.Add(user);

        if (user.InvitedFriends.Contains(friendUser))
        {
            user.InvitedFriends.Remove(friendUser);
        }

        if (friendUser.InvitedFriends.Contains(user))
        {
            friendUser.InvitedFriends.Remove(user);
        }

        await _context.SaveChangesAsync();
    }
}
