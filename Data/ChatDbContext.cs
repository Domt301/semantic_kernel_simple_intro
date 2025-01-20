using Microsoft.EntityFrameworkCore;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<ChatMessageDto> Messages { get; set; }
    public DbSet<ChatSessionDto> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    modelBuilder.Entity<ChatSessionDto>()
        .ToTable("chat_sessions")
        .HasKey(s => s.SessionId);

    modelBuilder.Entity<ChatMessageDto>()
        .ToTable("chat_messages")
        .HasKey(m => m.Id);

    // Define the relationship between ChatSessionDto and ChatMessageDto
    modelBuilder.Entity<ChatMessageDto>()
        .HasOne<ChatSessionDto>() // Indicates a relationship with ChatSessionDto
        .WithMany(s => s.Messages) // One session can have many messages
        .HasForeignKey(m => m.SessionId) // Use SessionId as the foreign key
        .HasPrincipalKey(s => s.SessionId); // Specify the principal key in ChatSessionDto

    // Additional index configuration
    modelBuilder.Entity<ChatMessageDto>()
        .HasIndex(m => m.SessionId);
    }
}