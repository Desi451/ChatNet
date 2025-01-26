using System.Collections.Concurrent;

namespace ChatNet.SignalR;

public class OnlineUsersService
{
    private readonly ConcurrentDictionary<string, string> _onlineUsers = new();
    private readonly ConcurrentDictionary<string, List<string>> _userGroups = new();

    public void AddUser(string connectionId, string userName)
    {
        _onlineUsers[connectionId] = userName;
        _userGroups[connectionId] = new List<string>();
    }

    public void RemoveUser(string connectionId)
    {
        _onlineUsers.TryRemove(connectionId, out _);
        _userGroups.TryRemove(connectionId, out _);
    }

    public IEnumerable<string> GetOnlineUsers()
    {
        return _onlineUsers.Values;
    }

    public void AddUserToGroup(string connectionId, string groupName)
    {
        if (_userGroups.TryGetValue(connectionId, out var groups))
        {
            groups.Add(groupName);
        }
    }

    public void RemoveUserFromGroup(string connectionId, string groupName)
    {
        if (_userGroups.TryGetValue(connectionId, out var groups))
        {
            groups.Remove(groupName);
        }
    }

    public List<string> GetUserGroups(string connectionId)
    {
        if (_userGroups.TryGetValue(connectionId, out var groups))
        {
            return groups;
        }
        return new List<string>();
    }
}
