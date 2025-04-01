using CodeSphere.Domain.Requests;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CodeSphere.Domain.Premitives
{
    public static class Helper
    {
        public static string ScriptFilePath;
        public const string PythonCompiler = "python:3.8-slim";
        public const string CppCompiler = "gcc:latest";
        public const string CSharpCompiler = "mcr.microsoft.com/dotnet/sdk:5.0";
        public const string ImagesDirectory = "UsersImages";

        public static string GenerateContestKey(int contestId)
        {
            return $"contest:{contestId}:standing";
        }
        public static string GenerateUserSubmissionKey(string userId, int contestId)
        {
            // key contains userid and contest id 
            return $"user:{userId}:contest:{contestId}:submissions";
        }
        public static string CreateExecuteCodeCommand(string containerId, decimal timeLimit)
        {
            string runTimeLimit = $"{timeLimit}s";
            //string runMemoryLimit = $"{Math.Round(memoryLimit)}mb";  // Round to nearest integer to avoid any decimals

            return $"docker exec {containerId} /usr/bin/bash /run_code.sh {runTimeLimit}";
        }
        static Helper()
        {
            ScriptFilePath = SetScriptFilePath();
        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static IEnumerable<T> DeserializeCollection<T>(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allows matching JSON property names with different cases
            };
            return JsonSerializer.Deserialize<IEnumerable<T>>(json, options);
        }


        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string SetScriptFilePath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);

            // Navigate up to the solution root
            while (directoryInfo != null && !DirectoryContainsFile(directoryInfo.FullName, "*.sln"))
            {
                directoryInfo = directoryInfo.Parent;
            }

            if (directoryInfo == null)
            {
                throw new Exception("Solution root not found.");
            }

            // Combine solution path with the Infrastructure project path
            string infrastructurePath = Path.Combine(directoryInfo.FullName, "CodeSphere.Domain", "Premitives", "run_code.sh");


            return infrastructurePath;

        }
        private static bool DirectoryContainsFile(string directoryPath, string searchPattern)
        {
            return Directory.GetFiles(directoryPath, searchPattern).Length > 0;
        }
        public static decimal ExtractExecutionTime(string time)
        {
            //"\nreal\t0m0.041s\nuser\t0m0.027s\nsys\t0m0.000s\n"
            //string temp = "";
            //bool found = false;
            //for (int i = 0; i < time.Length; i++)
            //{
            //    if (time[i] == 'm')
            //    {
            //        found = true;
            //        continue;
            //    }

            //    if (time[i] == 's' && found)
            //        break;
            //    if (found)
            //        temp += time[i];
            //}

            Match match = Regex.Match(time, @"real\t\d+m([\d.]+)s");
            string seconds = match.Groups[1].Value;

            if (Decimal.TryParse(seconds, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static decimal ExtractExecutionMemory(string memory)
        {
            // the string will be like "Memory Usage: 12345 KB"

            // get the number part in rgex 
            Match match = Regex.Match(memory, @"\d+");
            string memoryInKB = match.Value;

            if (Decimal.TryParse(memoryInKB, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static bool ValidateFile(Language fileType, decimal maxSizeInMb, decimal minSizeInMb, IFormFile file)
        {
            var extention = Path.GetExtension(file.FileName); // .cs,.cpp, .py, .java
            var size = file.Length;

            string requiredType = '.' + fileType.ToString();
            if (extention != requiredType)
                return false;

            if (size > maxSizeInMb * 1024 * 1024 || size < minSizeInMb * 1024 * 1024)
                return false;

            return true;
        }


        public static string ConvertUserToRedisMemeber(UserToCache user)
            => $"{user.UserId}|{user.UserName}|{user.ImagePath}";

        public static UserToCache ConvertRedisMemberToUser(string member)
        {
            // TODO : username may have | in it
            string[] parts = member.Split('|');
            return new UserToCache
            {
                UserId = parts[0],
                UserName = parts[1],
                ImagePath = parts[2]
            };
        }
    }
}
