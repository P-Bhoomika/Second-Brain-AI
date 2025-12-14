using System;
using Pgvector;

namespace SecondBrain.Infrastructure.Models;

public class Embedding
{
    // ✅ PRIMARY KEY
    public Guid ChunkId { get; set; }

    // ✅ pgvector column
    public Vector Vector { get; set; } = default!;

    public string Model { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}