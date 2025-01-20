public class ChatSessionDto
{
    public string SessionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<ChatMessageDto> Messages { get; set; } = new();
}