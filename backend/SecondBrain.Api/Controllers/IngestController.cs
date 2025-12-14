using System;
using Microsoft.AspNetCore.Mvc;
using SecondBrain.Api.DTOs;
using SecondBrain.Core.Models;
using SecondBrain.Infrastructure.Db;

namespace SecondBrain.Api.Controllers;

[ApiController]
[Route("ingest")]
public class IngestController : ControllerBase
{
    private readonly SecondBrainDbContext _dbContext;

    public IngestController(SecondBrainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("text")]
    public IActionResult IngestText([FromBody] IngestTextRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Text is required.");
        }

        var source = new Source
        {
            SourceId = Guid.NewGuid(),
            UserId = Guid.NewGuid(), // placeholder
            SourceType = "text",
            Title = request.Title,
            ObservedAt = request.ObservedAt ?? DateTime.UtcNow,
            IngestedAt = DateTime.UtcNow
        };

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

        try
        {
            _dbContext.Sources.Add(source);
            _dbContext.Chunks.AddRange(chunks);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Database is not available yet.",
                detail = ex.Message
            });
        }

        return Ok(new
        {
            sourceId = source.SourceId,
            chunksCreated = chunks.Count,
            message = "Text ingested and persisted successfully."
        });
    }
}
