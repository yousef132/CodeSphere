using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.CustomExceptions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using CodeSphere.Domain.Responses.SubmissionResponses;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class ExecutionService : IExecutionService
    {
        private readonly DockerClient _dockerClient;
        private readonly IFileService fileService;
        private readonly IUnitOfWork unitOfWork;
        private string _requestDirectory = null;
        private string _containerId = null;
        private string outputFile;
        private string errorFile;
        private string runTimeFile;
        private string runTimeErrorFile;
        private string memoryFile;
        public ExecutionService(IFileService fileService, IUnitOfWork unitOfWork)
        {
            _dockerClient = new DockerClientConfiguration(new Uri("tcp://localhost:2375")).CreateClient();
            _requestDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_requestDirectory);
            outputFile = Path.Combine(_requestDirectory, "output.txt");
            errorFile = Path.Combine(_requestDirectory, "error.txt");
            runTimeFile = Path.Combine(_requestDirectory, "runtime.txt");
            runTimeErrorFile = Path.Combine(_requestDirectory, "runtime_errors.txt");
            memoryFile = Path.Combine(_requestDirectory, "memory.txt");
            this.fileService = fileService;
            this.unitOfWork = unitOfWork;
        }


        public async Task<object> ExecuteCodeAsync(string code, Language language, List<CustomTestcaseDto> testcases, decimal runTimeLimit, decimal memoryLimit)
        {
            string path = await fileService.CreateCodeFile(code, language, _requestDirectory);
            List<object> results = new();
            try
            {
                // create container 
                await CreateAndStartContainer(language);

                await Task.Delay(10000);

                for (int i = 0; i < testcases.Count; i++)
                {

                    await fileService.CreateTestCasesFile(testcases[i].Input, _requestDirectory);

                    await ExecuteCodeInContainer(runTimeLimit, memoryLimit);

                    var result = await CalculateResult(testcases[i], runTimeLimit, code);

                    results.Add(result);
                }
                return results;
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


        }


        public async Task<object> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit, decimal memoryLimit)
        {
            string path = await fileService.CreateCodeFile(code, language, _requestDirectory);
            decimal maxRunTime = 0m;
            decimal maxMemory = 0m;
            try
            {
                // create container 
                await CreateAndStartContainer(language);

                await Task.Delay(10000);

                for (int i = 0; i < testCases.Count; i++)
                {
                    await fileService.CreateTestCasesFile(testCases[i].Input, _requestDirectory);

                    await ExecuteCodeInContainer(runTimeLimit, memoryLimit);

                    var result = await CalculateResult(testCases[i], runTimeLimit, code, i + 1, testCases[i].Input);

                    if (result.SubmissionResult != SubmissionResult.Accepted)
                        return result;

                    maxRunTime = Math.Max(maxRunTime, result.ExecutionTime);
                    maxMemory = Math.Max(maxMemory, result.ExecutionMemory);
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


            return new AcceptedResponse
            {
                ExecutionTime = maxRunTime,
                ExecutionMemory = maxMemory
            };

        }
        //TODO : use strategy patter instead of this function
        private async Task<BaseSubmissionResponse> CalculateResult(Testcase testCase, decimal runTimeLimit, string code, int testcaseNumber, string input)
        {
            string output = await fileService.ReadFileAsync(outputFile);
            string error = await fileService.ReadFileAsync(errorFile);
            string runTime = await fileService.ReadFileAsync(runTimeFile);
            string runTimeError = await fileService.ReadFileAsync(runTimeErrorFile);
            string memory = await fileService.ReadFileAsync(memoryFile);

            // Initialize the run result
            BaseSubmissionResponse response = default;

            if (!string.IsNullOrEmpty(error))
            {
                return new CompilationErrorResponse
                {
                    Message = error,
                    SubmissionResult = SubmissionResult.CompilationError,
                    ExecutionTime = 0m
                };
            }

            if (!string.IsNullOrEmpty(runTimeError))
            {
                return new RunTimeErrorResponse
                {
                    Message = runTimeError,
                    SubmissionResult = SubmissionResult.RunTimeError,
                    Input = input,
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    ExecutionTime = 0
                };
            }

            if (runTime?.Contains("TIMELIMITEXCEEDED") == true)
            {
                return new TimeLimitExceedResponse
                {
                    Input = input,
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    ExecutionTime = runTimeLimit,
                    SubmissionResult = SubmissionResult.TimeLimitExceeded
                };
            }

            //if (output?.Length > 0)
            //{
            if (output.TrimEnd('\n') != testCase.Output.TrimEnd('\n'))
            {
                return new WrongAnswerResponse
                {
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    ActualOutput = output,
                    Input = input,
                    ExpectedOutput = testCase.Output,
                    SubmissionResult = SubmissionResult.WrongAnswer,
                    ExecutionTime = Helper.ExtractExecutionTime(runTime)
                };
            }
            // }
            return new AcceptedResponse
            {
                NumberOfPassedTestCases = testcaseNumber,
                ExecutionTime = Helper.ExtractExecutionTime(runTime),
                ExecutionMemory = Helper.ExtractExecutionMemory(memory)
            };
        }


        private async Task<BaseSubmissionResponse> CalculateResult(CustomTestcaseDto testcaseDto, decimal runTimeLimit, string code)
        {
            string output = await fileService.ReadFileAsync(outputFile);
            string error = await fileService.ReadFileAsync(errorFile);
            string runTime = await fileService.ReadFileAsync(runTimeFile);
            string runTimeError = await fileService.ReadFileAsync(runTimeErrorFile);

            // Initialize the run result
            BaseSubmissionResponse response = default;

            if (!string.IsNullOrEmpty(error))
            {
                return new CompilationErrorResponse
                {
                    Message = error,
                    SubmissionResult = SubmissionResult.CompilationError,
                    ExecutionTime = 0m
                };
            }

            if (!string.IsNullOrEmpty(runTimeError))
            {
                return new RunTimeErrorResponse
                {
                    Message = runTimeError,
                    SubmissionResult = SubmissionResult.RunTimeError,
                    ExecutionTime = Helper.ExtractExecutionTime(runTime)
                };
            }

            if (runTime?.Contains("TIMELIMITEXCEEDED") == true)
            {
                return new TimeLimitExceedResponse
                {
                    ExecutionTime = runTimeLimit,
                    SubmissionResult = SubmissionResult.TimeLimitExceeded,
                };
            }

            // if (output?.Length > 0)
            // {
            if (output.TrimEnd('\n') != testcaseDto.ExpectedOutput.TrimEnd('\n'))
            {
                return new WrongAnswerResponse
                {
                    ActualOutput = output,
                    ExpectedOutput = testcaseDto.ExpectedOutput,
                    SubmissionResult = SubmissionResult.WrongAnswer,
                    ExecutionTime = Helper.ExtractExecutionTime(runTime)
                };
            }
            //  }
            return new AcceptedResponse
            {
                ExecutionTime = Helper.ExtractExecutionTime(runTime),
                ExecutionMemory = 3m,
            };
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

            string s = Helper.ScriptFilePath;

            var createContainerResponse = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                HostConfig = new HostConfig
                {
                    Binds = new[] { $"{_requestDirectory}:/code", $"{Helper.ScriptFilePath}:/run_code.sh" },
                    NetworkMode = "bridge",
                    Memory = 256 * 1024 * 1024,
                    AutoRemove = false
                },
                Name = "code_container",
                Image = image,
                Cmd = new[] { "sh", "-c", "apt-get update && apt-get install -y time && tail -f /dev/null" }, // Install time package

            });

            _containerId = createContainerResponse.ID;
            await _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
        }

        private async Task ExecuteCodeInContainer(decimal timeLimit, decimal memoryLimit)
        {

            string command = Helper.CreateExecuteCodeCommand(_containerId, timeLimit, memoryLimit);

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
