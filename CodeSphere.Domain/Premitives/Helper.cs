namespace CodeSphere.Domain.Premitives
{
    public static class Helper
    {
        public const string ScriptFilePath = @"D:\\Graduation Project\\Project\\CodeSphere\\CodeSphere\\CodeSphere.Domain\\Premitives\\run_code.sh";
        public const string PythonCompiler = "python:3.8-slim";
        public const string CppCompiler = "gcc:latest";
        public const string CSharpCompiler = "mcr.microsoft.com/dotnet/sdk:5.0";

        public static string ExecuteCodeCommand(string containerId, decimal runTime)
        {
            string runTimeLimit = $"{runTime}s";
            // Prepare the docker exec command to run the script inside the container
            return $"docker exec {containerId} /usr/bin/bash /run_code.sh {runTimeLimit}";
        }
    }

}
