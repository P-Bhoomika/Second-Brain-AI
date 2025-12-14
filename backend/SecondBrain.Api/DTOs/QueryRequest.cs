using System;

namespace SecondBrain.Api.DTOs;

public class QueryRequest
{
    public string Question { get; set; } = string.Empty;

    // Optional time filter (future use)
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}