using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class FileService : IFileService
    {
        public bool CheckFileExtension(IFormFile file, Language language)
        {
            var extension = System.IO.Path.GetExtension(file.FileName);
            if (extension == null)
                return false;

            extension = extension[1..];

            bool found = false;
            // find the supported language and compare it with the file extension
            foreach (Language lan in Enum.GetValues(typeof(Language)))
                if (lan == language && extension == lan.ToString())
                    found = true;

            return found;
        }
        public async Task<string> ReadFileAsync(string filePath)
               => await System.IO.File.ReadAllTextAsync(filePath);


        public async Task<string> CreateTestCasesFile(string testCase, string requestDirectory)
        {
            string testCasesPath = Path.Combine(requestDirectory, "testcases.txt");
            await System.IO.File.WriteAllTextAsync(testCasesPath, testCase);
            return testCasesPath;
        }

        public async Task<string> CreateCodeFile(string code, Language language, string requestDirectory)
        {

            string testCasesPath = Path.Combine(requestDirectory, $"main.{language.ToString()}");

            await System.IO.File.WriteAllTextAsync(testCasesPath, code);
            return testCasesPath;
        }
    }
}
