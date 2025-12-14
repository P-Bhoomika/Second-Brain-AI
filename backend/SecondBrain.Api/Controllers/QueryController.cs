using Microsoft.AspNetCore.Mvc;
using SecondBrain.Api.DTOs;

namespace SecondBrain.Api.Controllers;

[ApiController]
[Route("query")]
public class QueryController : ControllerBase
{
    [HttpPost]
    public IActionResult Query([FromBody] QueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("Question is required.");
        }

        // Placeholder response (AI comes later)
        return Ok(new
        {
            question = request.Question,
            answer = "Query received. Retrieval and AI synthesis coming next."
        });
    }
}
