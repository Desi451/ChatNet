using ChatNet.Domain.Entity;

namespace ChatNet.Services.Interfaces;

public interface IChatService
{
    Task AddMessage(Message message);
    Task<List<Message>> GetMessagesForChat(int userId, int chatWithId);
}
