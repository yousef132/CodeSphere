using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("problems")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProblemResponse>>> GetProblemsForBlog()
        {
            var problems = await _unitOfWork.BlogRepository.GetProblemsForBlogAsync();

            var problemResponses = _mapper.Map<IEnumerable<ProblemResponse>>(problems);

            return Ok(problemResponses);
        }

    }
}
