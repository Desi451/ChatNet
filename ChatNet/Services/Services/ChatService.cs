using ChatNet.Database;
using ChatNet.Domain.Entity;
using ChatNet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatNet.Services.Services;

public class ChatService : IChatService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatService(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;

    }
    public async Task AddMessage(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetMessagesForChat(int userId, int chatWithId)
    {
        return await _context.Messages
            .Where(u =>
            (u.Sender.Id == userId && u.Reciver.Id == chatWithId) ||
            (u.Sender.Id == chatWithId && u.Reciver.Id == userId)
            ).ToListAsync();
    }
}
