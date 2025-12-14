using System;

namespace SecondBrain.Core.Models;

public class Chunk
{
    public Guid ChunkId { get; set; }
    public Guid SourceId { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ChunkIndex { get; set; }
    public DateTime ObservedAt { get; set; }
    public int TokenCount { get; set; }
}