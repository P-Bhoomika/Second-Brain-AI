using System;

namespace SecondBrain.Core.Models;

public class Source
{
    public Guid SourceId { get; set; }
    public Guid UserId { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Uri { get; set; }
    public DateTime IngestedAt { get; set; } = DateTime.UtcNow;
    public DateTime ObservedAt { get; set; }
    public string? ContentHash { get; set; }
}