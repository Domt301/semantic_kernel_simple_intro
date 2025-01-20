using System.ComponentModel.DataAnnotations.Schema;

public class ChatSessionDto
{
    [Column("session_id")]
    public string SessionId { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("last_updated")]
    public DateTime LastUpdated { get; set; }
    
    // Navigation property - not mapped to a column
    public List<ChatMessageDto> Messages { get; set; } = new();
}