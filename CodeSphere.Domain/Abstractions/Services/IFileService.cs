using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IFileService
    {
        bool CheckFileExtension(IFormFile file, Language language);

        Task<string> UploadFileAsync(IFormFile file, string directory);
        Task<string> ReadFile(IFormFile file);
        Task<string> ReadFileAsync(string filePath);
        Task<string> UpdateTestCasesFileAsync(string testCasesContent, string filePathOrDirectory);

        Task<string> CreateTestCasesFile(string testCase, string requestDirectory);
        Task<string> CreateCodeFile(string code, Language language, string requestDirectory);
    }
}
