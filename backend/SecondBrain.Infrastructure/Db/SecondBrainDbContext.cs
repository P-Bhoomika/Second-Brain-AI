using Microsoft.EntityFrameworkCore;
using SecondBrain.Core.Models;
using SecondBrain.Infrastructure.Models;

namespace SecondBrain.Infrastructure.Db;

public class SecondBrainDbContext : DbContext
{
    public SecondBrainDbContext(DbContextOptions<SecondBrainDbContext> options)
        : base(options) { }

    public DbSet<Chunk> Chunks => Set<Chunk>();
    public DbSet<Embedding> Embeddings => Set<Embedding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ✅ PRIMARY KEY
        modelBuilder.Entity<Embedding>()
            .HasKey(e => e.ChunkId);

        // ✅ Map pgvector column
        modelBuilder.Entity<Embedding>()
            .Property(e => e.Vector)
            .HasColumnType("vector(384)");
    }
}
