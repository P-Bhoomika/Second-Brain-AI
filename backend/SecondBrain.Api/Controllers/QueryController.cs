using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondBrain.Api.DTOs;
using SecondBrain.Api.Services;
using SecondBrain.Infrastructure.Db;
using SecondBrain.Core.Models;

[ApiController]
[Route("query")]
public class QueryController : ControllerBase
{
    private readonly SecondBrainDbContext _dbContext;
    private readonly IEmbeddingService _embeddingService;

    public QueryController(
        SecondBrainDbContext dbContext,
        IEmbeddingService embeddingService)
    {
        _dbContext = dbContext;
        _embeddingService = embeddingService;
    }

    [HttpPost]
    public async Task<IActionResult> Query([FromBody] QueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest("Question is required.");

        // 1️⃣ Generate embedding (fake or real)
        var queryEmbedding = await _embeddingService
            .CreateEmbeddingAsync(request.Question);

        // 2️⃣ Apply temporal filters
        var from = request.From ?? DateTime.MinValue;
        var to = request.To ?? DateTime.MaxValue;

        // 3️⃣ Semantic search via pgvector SQL operator
        var results = await _dbContext.Chunks
     .FromSqlRaw(@"
        SELECT c.*
        FROM ""Chunks"" c
        JOIN ""Embeddings"" e ON c.""ChunkId"" = e.""ChunkId""
        WHERE c.""ObservedAt"" BETWEEN {0} AND {1}
        ORDER BY e.""Vector"" <=> ({2}::vector)
        LIMIT 5
    ",
     from,
     to,
     queryEmbedding)
     .Select(c => new
     {
         c.Text,
         c.ObservedAt
     })
     .ToListAsync();

        return Ok(new
        {
            question = request.Question,
            results,
            message = "Semantic search successful"
        });
    }
}
