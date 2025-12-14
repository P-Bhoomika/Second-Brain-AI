using Microsoft.AspNetCore.Mvc;
using SecondBrain.Api.DTOs;
using SecondBrain.Infrastructure.Db;

namespace SecondBrain.Api.Controllers;

[ApiController]
[Route("query")]
public class QueryController : ControllerBase
{
    private readonly SecondBrainDbContext _dbContext;

    public QueryController(SecondBrainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public IActionResult Query([FromBody] QueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("Question is required.");
        }

        // We are not querying the DB yet – just proving access
        var chunkCount =  0;

        return Ok(new
        {
            question = request.Question,
            chunksInSystem = chunkCount,
            answer = "DbContext injected successfully. Retrieval logic coming next."
        });
    }
}
