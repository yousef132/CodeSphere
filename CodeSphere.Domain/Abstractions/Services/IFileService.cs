using CodeSphere.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IFileService
    {
        public bool CheckFileExtension(IFormFile file, Language language);

        public Task<string> ReadFileAsync(string filePath);

        public Task<string> CreateTestCasesFile(string testCase, string requestDirectory);
        public Task<string> CreateCodeFile(string code, Language language, string requestDirectory);
    }
}
