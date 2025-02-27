using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class PlagiarismController : BaseController
    {
        private readonly IPlagiarismService plagiarismService;
        public PlagiarismController(IPlagiarismService plagiarismService)
        {
            this.plagiarismService = plagiarismService;
        }

        [HttpPost]
        public IActionResult GetSimilarity([FromBody] CodeSimilarityRequest request)
        {
            return Ok(plagiarismService.GetSimilarity(request.Code1, request.Code2));
        }
    }
}
