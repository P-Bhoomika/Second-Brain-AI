using System;
using Microsoft.AspNetCore.Mvc;
using SecondBrain.Api.DTOs;
using SecondBrain.Core.Models;

namespace SecondBrain.Api.Controllers;

[ApiController]
[Route("ingest")]
public class IngestController : ControllerBase
{
    [HttpPost("text")]
    public IActionResult IngestText([FromBody] IngestTextRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Text is required.");
        }

        // Placeholder logic (DB insert comes next)
        var source = new Source
        {
            SourceId = Guid.NewGuid(),
            UserId = Guid.NewGuid(), // placeholder
            SourceType = "text",
            Title = request.Title,
            ObservedAt = request.ObservedAt ?? DateTime.UtcNow
        };

        // Simple chunking (naive)
        var chunks = request.Text
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select((text, index) => new Chunk
            {
                ChunkId = Guid.NewGuid(),
                SourceId = source.SourceId,
                UserId = source.UserId,
                Text = text.Trim(),
                ChunkIndex = index,
                ObservedAt = source.ObservedAt
            })
            .ToList();

        return Ok(new
        {
            sourceId = source.SourceId,
            chunksCreated = chunks.Count,
            message = "Text ingested successfully (persistence coming next)."
        });
    }
}
