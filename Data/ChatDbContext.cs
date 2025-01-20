using Microsoft.EntityFrameworkCore;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<ChatMessageDto> Messages { get; set; }
    public DbSet<ChatSessionDto> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatSessionDto>()
            .HasKey(s => s.SessionId);

        modelBuilder.Entity<ChatMessageDto>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<ChatMessageDto>()
            .HasIndex(m => m.SessionId);
    }
}