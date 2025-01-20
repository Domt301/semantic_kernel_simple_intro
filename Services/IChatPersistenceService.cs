public interface IChatPersistenceService
{
    Task<string> CreateNewSessionAsync();
    Task<ChatSessionDto> GetSessionAsync(string sessionId);
    Task SaveMessageAsync(string sessionId, ChatMessageDto message);
    Task<Microsoft.SemanticKernel.ChatCompletion.ChatHistory> LoadChatHistoryAsync(string sessionId);
}