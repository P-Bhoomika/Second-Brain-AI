using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using SecondBrain.Api.Services;
using SecondBrain.Infrastructure.Db;
using SecondBrain.Infrastructure.Models;

namespace SecondBrain.Api.Controllers;

[ApiController]
[Route("embeddings")]
public class EmbeddingsController : ControllerBase
{
    private readonly SecondBrainDbContext _db;
    private readonly IEmbeddingService _embeddingService;

    public EmbeddingsController(
        SecondBrainDbContext db,
        IEmbeddingService embeddingService)
    {
        _db = db;
        _embeddingService = embeddingService;
    }

    [HttpPost("backfill")]
    public async Task<IActionResult> Backfill(int limit = 10)
    {
        var chunks = await _db.Chunks
            .Where(c => !_db.Embeddings.Any(e => e.ChunkId == c.ChunkId))
            .OrderBy(c => c.ObservedAt)
            .Take(limit)
            .ToListAsync();

        foreach (var chunk in chunks)
        {
            // ✅ float[]
            var floats = await _embeddingService.CreateEmbeddingAsync(chunk.Text);

            // ✅ convert ONLY here
            var embedding = new Embedding
            {
                ChunkId = chunk.ChunkId,
                Vector = new Vector(floats), // ✅ THIS WAS THE BUG
                Model = "fake",
                CreatedAt = DateTime.UtcNow
            };

            _db.Embeddings.Add(embedding);
        }

        await _db.SaveChangesAsync();

        return Ok(new { embeddingsCreated = chunks.Count });
    }
}
