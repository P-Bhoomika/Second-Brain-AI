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
            return BadRequest("Text is required.");

        var sourceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var observedAt = request.ObservedAt ?? DateTime.UtcNow;

        var chunks = request.Text
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select((text, index) => new Chunk
            {
                ChunkId = Guid.NewGuid(),
                SourceId = sourceId,
                UserId = userId,
                Text = text.Trim(),
                ChunkIndex = index,
                ObservedAt = observedAt,
                TokenCount = text.Length
            })
            .ToList();

        _dbContext.Chunks.AddRange(chunks);
        _dbContext.SaveChanges();

        return Ok(new
        {
            sourceId,
            chunksCreated = chunks.Count,
            message = "Text ingested successfully."
        });
    }
}
