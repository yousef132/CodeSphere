using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IFileService
    {
        bool CheckFileExtension(IFormFile file, Language language);

        Task<string> ReadFile(IFormFile file);
        Task<string> ReadFileAsync(string filePath);

        Task<string> CreateTestCasesFile(string testCase, string requestDirectory);
        Task<string> CreateCodeFile(string code, Language language, string requestDirectory);
    }
}
