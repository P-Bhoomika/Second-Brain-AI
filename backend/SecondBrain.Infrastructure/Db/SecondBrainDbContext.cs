using Microsoft.EntityFrameworkCore;
using SecondBrain.Core.Models;

namespace SecondBrain.Infrastructure.Db;

public class SecondBrainDbContext : DbContext
{
    public SecondBrainDbContext(DbContextOptions<SecondBrainDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Source> Sources => Set<Source>();
    public DbSet<Chunk> Chunks => Set<Chunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<Source>()
            .HasKey(s => s.SourceId);

        modelBuilder.Entity<Chunk>()
            .HasKey(c => c.ChunkId);

        base.OnModelCreating(modelBuilder);
    }
}