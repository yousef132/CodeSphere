using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //[HttpGet("problems")]
        //public async Task<IActionResult<IEnumerable<Problem>>> GetProblems()
        //{
        //    var problems = await _unitOfWork.BlogRepository
        //} 
    }
}
