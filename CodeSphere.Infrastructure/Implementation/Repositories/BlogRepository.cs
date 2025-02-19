using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Problem>> GetProblemsForBlogAsync() =>
            await _dbContext.Problems.ToListAsync();
           
        public async Task AddProblemToBlogAsync(int blogId, int problemId)
        {
            throw new NotImplementedException();

            // var blog = await _dbContext.Blogs.FindAsync(blogId);    
        }

        public Task AddSolutionToBlogAsync(int blogId, string solutionContent)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Blog>> GetBlogsByProblemIdAsync(int problemId)
        {
            throw new NotImplementedException();
        }

    }
}
