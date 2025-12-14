using System;

namespace SecondBrain.Api.DTOs;

public class IngestTextRequest
{
    public string Text { get; set; } = string.Empty;
    public string? Title { get; set; }
    public DateTime? ObservedAt { get; set; }
}