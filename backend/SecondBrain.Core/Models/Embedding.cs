using System;

namespace SecondBrain.Core.Models;

public class Embedding
{
    public Guid ChunkId { get; set; }

    public float[] Vector { get; set; } = Array.Empty<float>();

    public string Model { get; set; } = "text-embedding-3-small";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}