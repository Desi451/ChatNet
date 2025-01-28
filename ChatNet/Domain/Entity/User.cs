using ChatNet.Services.Helpers;

namespace ChatNet.Domain.Entity;

public class User
{
    public User() { }

    public User(string username, string password, string email)
    {
        Name = username;
        Password = PasswordHelper.HashPassword(password);
        Email = email;
        AllowInvites = true;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public bool AllowInvites { get; private set; }

    public List<User> Friends { get; private set; } = [];
    public List<User> InvitedFriends { get; private set; } = [];
}
