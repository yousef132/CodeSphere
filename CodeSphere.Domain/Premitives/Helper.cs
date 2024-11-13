using System.Text.RegularExpressions;

namespace CodeSphere.Domain.Premitives
{
    public static class Helper
    {
        public const string ScriptFilePath = @"D:\\Graduation Project\\Project\\CodeSphere\\CodeSphere\\CodeSphere.Domain\\Premitives\\run_code.sh";
        public const string PythonCompiler = "python:3.8-slim";
        public const string CppCompiler = "gcc:latest";
        public const string CSharpCompiler = "mcr.microsoft.com/dotnet/sdk:5.0";

        //public static string ExecuteCodeCommand(string containerId, decimal timeLimit,decimal memoryLimit)
        //{
        //    string runTimeLimit = $"{timeLimit}s";
        //    string runMemoryLimit = $"{memoryLimit}mb";
        //    // Prepare the docker exec command to run the script inside the container
        //    return $"docker exec {containerId} /usr/bin/bash /run_code.sh {runTimeLimit} {runMemoryLimit}";
        //}
        public static string CreateExecuteCodeCommand(string containerId, decimal timeLimit, decimal memoryLimit)
        {
            string runTimeLimit = $"{timeLimit}s";
            string runMemoryLimit = $"{Math.Round(memoryLimit)}mb";  // Round to nearest integer to avoid any decimals

            return $"docker exec {containerId} /usr/bin/bash /run_code.sh {runTimeLimit} {runMemoryLimit}";
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

            return decimal.Parse(seconds);
        }

    }
}
