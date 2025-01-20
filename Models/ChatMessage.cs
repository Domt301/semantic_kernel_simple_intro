using System.ComponentModel.DataAnnotations.Schema;

public class ChatMessageDto
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("session_id")]
    public string? SessionId { get; set; }
    
    [Column("role")]
    public string? Role { get; set; }
    
    [Column("content")]
    public string? Content { get; set; }
    
    [Column("timestamp")]
    public DateTime Timestamp { get; set; }
}