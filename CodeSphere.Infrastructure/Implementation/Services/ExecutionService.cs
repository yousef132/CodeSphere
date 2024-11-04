using CodeSphere.Application.Features.Problem.Commands.SolveProblem;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.CustomExceptions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class ExecutionService : IExecutionService
    {
        private readonly DockerClient _dockerClient;
        private readonly IFileService fileService;
        private string _requestDirectory = null;
        private string _containerId = null;
        private string outputFile;
        private string errorFile;
        private string runTimeFile;
        private string runTimeErrorFile;
        public ExecutionService(IFileService fileService)
        {
            _dockerClient = new DockerClientConfiguration(new Uri("tcp://localhost:2375")).CreateClient();
            _requestDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_requestDirectory);
            outputFile = Path.Combine(_requestDirectory, "output.txt");
            errorFile = Path.Combine(_requestDirectory, "error.txt");
            runTimeFile = Path.Combine(_requestDirectory, "runtime.txt");
            runTimeErrorFile = Path.Combine(_requestDirectory, "runtime_errors.txt");
            this.fileService = fileService;
        }

        public async Task<List<TestCaseRunResult>> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit)
        {


            string path = await fileService.CreateCodeFile(code, language, _requestDirectory);

            var results = new List<TestCaseRunResult>();
            try
            {

                // create container 
                await CreateAndStartContainer(language);
                for (int i = 0; i < testCases.Count; i++)
                {
                    // for each testcase run the code against it and capture the output
                    // of the program and compare it with expected output
                    testCases[i].Input = testCases[i].Input.Replace("\\n", "\n");
                    testCases[i].Output = testCases[i].Output.Replace("\\n", "\n");

                    await fileService.CreateTestCasesFile(testCases[i].Input, _requestDirectory);

                    await ExecuteCodeInContainer(runTimeLimit);

                    var result = await CalculateResult(testCases[i], runTimeLimit);

                    results.Add(result);

                    if (result.Result != SubmissionResult.Accepted)
                        return results;


                }
            }
            catch (Exception ex)
            {
                throw new CodeExecutionException("Error while running testcases !!!");
            }
            finally
            {
                if (Directory.Exists(_requestDirectory))
                {
                    Directory.Delete(_requestDirectory, true);
                }

                if (_containerId != null)
                {
                    await _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters { Force = true });
                }
            }
            return results;
        }

        private async Task<TestCaseRunResult> CalculateResult(Testcase testCase, decimal runTimeLimit)
        {
            string output = await fileService.ReadFileAsync(outputFile);
            string error = await fileService.ReadFileAsync(errorFile);
            string runTime = await fileService.ReadFileAsync(runTimeFile);
            string runTimeError = await fileService.ReadFileAsync(runTimeErrorFile);

            // Initialize the run result
            var runResult = new TestCaseRunResult
            {
                TestCaseId = testCase.Id,
                Input = testCase.Input,
                ExpectedOutput = testCase.Output,
            };

            if (!string.IsNullOrEmpty(error))
            {
                return SetResult(runResult, error, SubmissionResult.CompilationError);
            }

            if (!string.IsNullOrEmpty(runTimeError))
            {
                return SetResult(runResult, runTimeError, SubmissionResult.RunTimeError);
            }

            if (runTime?.Contains("TIMELIMITEXCEEDED") == true)
            {
                runResult.Result = SubmissionResult.TimeLimitExceeded;
                runResult.RunTime = runTimeLimit;
                return runResult;
            }

            runResult.ActualOutput = output;
            runResult.RunTime = 1m; // Replace with actual runtime if available

            // Compare actual output with expected output
            runResult.Result = testCase.Output != runResult.ActualOutput
                ? SubmissionResult.WrongAnswer
                : SubmissionResult.Accepted;

            return runResult;
        }

        private TestCaseRunResult SetResult(TestCaseRunResult runResult, string error, SubmissionResult result)
        {
            runResult.Error = error;
            runResult.Result = result;
            return runResult;
        }
        private async Task CreateAndStartContainer(Language language)
        {
            var image = language switch
            {
                Language.py => Helper.PythonCompiler,
                Language.cpp => Helper.CppCompiler,
                Language.cs => Helper.CSharpCompiler,
                _ => throw new ArgumentException("Unsupported language")
            };

            var createContainerResponse = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                HostConfig = new HostConfig
                {
                    Binds = new[] { $"{_requestDirectory}:/code", $"{Helper.ScriptFilePath}:/run_code.sh" },
                    NetworkMode = "none",
                    Memory = 256 * 1024 * 1024,
                    AutoRemove = false
                },
                Name = "code_container",
                Image = image,
                Cmd = new[] { "tail", "-f", "/dev/null" },

            });
            _containerId = createContainerResponse.ID;
            await _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
        }
        private async Task ExecuteCodeInContainer(decimal runTime)
        {

            string command = Helper.ExecuteCodeCommand(_containerId, runTime);

            // Start the process to execute the command
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                try
                {
                    process.Start();
                    process.WaitForExit();

                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    throw new CodeExecutionException("Error While Executing Client Code !!");
                }
            }
        }

    }
}
