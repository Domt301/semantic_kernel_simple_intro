public class ChatMessageDto
{
    public int Id { get; set; }
    public string SessionId { get; set; }
    public string Role { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
}