using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class FileService : IFileService
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
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

        public async Task<string> UpdateTestCasesFileAsync(string testCasesContent, string filePathOrDirectory)
        {
            string filePath = filePathOrDirectory;

            // If it's a directory, append "testcases.txt"
            if (Directory.Exists(filePathOrDirectory))
            {
                filePath = Path.Combine(filePathOrDirectory, "testcases.txt");
            }

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(filePath)!;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Overwrite file from the beginning
            await File.WriteAllTextAsync(filePath, testCasesContent);
            return filePath;
        }


        public async Task<string> CreateCodeFile(string code, Language language, string requestDirectory)
        {

            string testCasesPath = Path.Combine(requestDirectory, $"main.{language.ToString()}");

            await System.IO.File.WriteAllTextAsync(testCasesPath, code);
            return testCasesPath;
        }

        public async Task<string> ReadFile(IFormFile file)
        {
            string content;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                content = await reader.ReadToEndAsync();
            }
            return content;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string directory)
        {
            var serverPath = webHostEnvironment.WebRootPath;
            var extension = Path.GetExtension(file.FileName);
            var imagePath = $"/{directory}/{Guid.NewGuid().ToString().Replace("-", string.Empty)}{extension}";

            var fullPath = serverPath + imagePath;

            if (file.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(serverPath))
                        // if directory not found then make one
                        Directory.CreateDirectory(serverPath);

                    using (FileStream stream = File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                        await stream.FlushAsync();
                        return imagePath;
                    }
                }
                catch (Exception ex)
                {
                    return "FailedToUploadImage";
                    throw;
                }

            }
            else
                return "NoImage";
        }
    }
}
