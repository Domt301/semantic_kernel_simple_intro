using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;

public class ChatPersistenceService : IChatPersistenceService
{
    private readonly ChatDbContext _context;

    public ChatPersistenceService(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateNewSessionAsync()
    {
        var sessionId = Guid.NewGuid().ToString();
        var session = new ChatSessionDto
        {
            SessionId = sessionId,
            CreatedAt = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return sessionId;
    }

    public async Task<ChatSessionDto> GetSessionAsync(string sessionId)
    {
        return await _context.Sessions
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    public async Task SaveMessageAsync(string sessionId, ChatMessageDto message)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null)
            throw new ArgumentException("Session not found", nameof(sessionId));

        message.SessionId = sessionId;
        message.Timestamp = DateTime.UtcNow;

        session.LastUpdated = DateTime.UtcNow;
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<ChatHistory> LoadChatHistoryAsync(string sessionId)
    {
        var messages = await _context.Messages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();

        var chatHistory = new ChatHistory();
        foreach (var message in messages)
        {
            switch (message.Role.ToLower())
            {
                case "user":
                    chatHistory.AddUserMessage(message.Content);
                    break;
                case "assistant":
                    chatHistory.AddAssistantMessage(message.Content);
                    break;
                case "system":
                    chatHistory.AddSystemMessage(message.Content);
                    break;
            }
        }

        return chatHistory;
    }
}