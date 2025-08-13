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
        public ExecutionService(IFileService fileService, IUnitOfWork unitOfWork)
        {
            _dockerClient = new DockerClientConfiguration(new Uri("tcp://localhost:2375")).CreateClient();
            _requestDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_requestDirectory);
            outputFile = Path.Combine(_requestDirectory, "output.txt");
            errorFile = Path.Combine(_requestDirectory, "error.txt");
            runTimeFile = Path.Combine(_requestDirectory, "runtime.txt");
            runTimeErrorFile = Path.Combine(_requestDirectory, "runtime_errors.txt");
            this.fileService = fileService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<object> ExecuteCodeAsync(string code, Language language, List<CustomTestcaseDto> testcases, decimal runTimeLimit)
        {
            string path = await fileService.CreateCodeFile(code, language, _requestDirectory);
            List<object> results = new();
            try
            {
                // create container 
                await CreateAndStartContainer(language);
                //await fileService.CreateTestCasesFile(testcases[i].Input, _requestDirectory);

                for (int i = 0; i < testcases.Count; i++)
                {

                    await fileService.CreateTestCasesFile(testcases[i].Input, _requestDirectory);
                    //await fileService.UpdateTestCasesFileAsync(testcases[i].Input, _requestDirectory);

                    await ExecuteCodeInContainer(runTimeLimit);

                    object result = await CalculateResult(testcases[i], i + 1, runTimeLimit, code, testcases.Count);

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

        private async Task<object> CalculateResult(CustomTestcaseDto testcaseDto, int testcaseNumber, decimal runTimeLimit, string code, int totalTestcases)
        {
            string output = await fileService.ReadFileAsync(outputFile);
            string error = await fileService.ReadFileAsync(errorFile);
            string runTime = await fileService.ReadFileAsync(runTimeFile);
            string runTimeError = await fileService.ReadFileAsync(runTimeErrorFile);


            BaseSubmissionResponse response = default;

            if (!string.IsNullOrEmpty(error))
            {
                return new CompilationErrorResponse
                {
                    Message = error,
                    SubmissionResult = SubmissionResult.CompilationError,
                    NumberOfPassedTestCases = 0,
                    TotalTestcases = totalTestcases
                };

            }

            if (!string.IsNullOrEmpty(runTimeError))
            {
                return new RunTimeErrorResponse
                {
                    Message = runTimeError,
                    SubmissionResult = SubmissionResult.RunTimeError,
                    Input = testcaseDto.Input,
                    TotalTestcases = totalTestcases,
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    ExpectedOutput = testcaseDto.ExpectedOutput

                };
            }


            if (runTime?.Contains("TIMELIMITEXCEEDED") == true)
            {
                return new TimeLimitExceedResponse
                {
                    Input = testcaseDto.Input,
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    TotalTestcases = totalTestcases,
                    SubmissionResult = SubmissionResult.TimeLimitExceeded,
                    ExpectedOutput = testcaseDto.ExpectedOutput,

                };
            }

            if (output.TrimEnd('\n') != testcaseDto.ExpectedOutput.TrimEnd('\n'))
            {
                return new WrongAnswerResponse
                {
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    TotalTestcases = totalTestcases,
                    ActualOutput = output,
                    Input = testcaseDto.Input,
                    ExpectedOutput = testcaseDto.ExpectedOutput,
                    SubmissionResult = SubmissionResult.WrongAnswer,
                };
            }

            return new AcceptedResponse
            {
                NumberOfPassedTestCases = testcaseNumber,
                ExecutionTime = Helper.ExtractExecutionTime(runTime),
                TotalTestcases = totalTestcases,
            };

        }

        public async Task<object> ExecuteCodeAsync(string code, Language language, List<Testcase> testCases, decimal runTimeLimit)
        {
            string path = await fileService.CreateCodeFile(code, language, _requestDirectory);
            decimal maxRunTime = 0m;
            try
            {
                await CreateAndStartContainer(language);

                for (int i = 0; i < testCases.Count; i++)
                {
                    await fileService.CreateTestCasesFile(testCases[i].Input, _requestDirectory);

                    await ExecuteCodeInContainer(runTimeLimit);

                    var result = await CalculateResult(testCases[i], runTimeLimit, code, i + 1, testCases.Count);

                    BaseSubmissionResponse baseResponse = result as BaseSubmissionResponse;
                    if (baseResponse.SubmissionResult != SubmissionResult.Accepted)
                        return result;

                    AcceptedResponse response = new(baseResponse);
                    maxRunTime = Math.Max(maxRunTime, response.ExecutionTime);
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
                NumberOfPassedTestCases = testCases.Count,
                TotalTestcases = testCases.Count
            };

        }

        //TODO : use strategy patter instead of this function
        private async Task<object> CalculateResult(Testcase testCase, decimal runTimeLimit, string code, int testcaseNumber, int totalTestcases)
        {

            string output = await fileService.ReadFileAsync(outputFile);
            string error = await fileService.ReadFileAsync(errorFile);
            string runTime = await fileService.ReadFileAsync(runTimeFile);
            string runTimeError = await fileService.ReadFileAsync(runTimeErrorFile);


            BaseSubmissionResponse response = default;

            if (!string.IsNullOrEmpty(error))
            {
                return new CompilationErrorResponse
                {
                    Message = error,
                    SubmissionResult = SubmissionResult.CompilationError,
                    NumberOfPassedTestCases = 0,
                    TotalTestcases = totalTestcases
                };

            }

            if (!string.IsNullOrEmpty(runTimeError))
            {
                return new RunTimeErrorResponse
                {
                    Message = runTimeError,
                    SubmissionResult = SubmissionResult.RunTimeError,
                    Input = testCase.Input,
                    TotalTestcases = totalTestcases,
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    ExpectedOutput = testCase.Output

                };
            }

            if (runTime?.Contains("TIMELIMITEXCEEDED") == true)
            {
                return new TimeLimitExceedResponse
                {
                    Input = testCase.Input,
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    TotalTestcases = totalTestcases,
                    SubmissionResult = SubmissionResult.TimeLimitExceeded,
                    ExpectedOutput = testCase.Output,

                };
            }

            if (output.TrimEnd('\n') != testCase.Output.TrimEnd('\n'))
            {
                return new WrongAnswerResponse
                {
                    NumberOfPassedTestCases = testcaseNumber - 1,
                    TotalTestcases = totalTestcases,
                    ActualOutput = output,
                    Input = testCase.Input,
                    ExpectedOutput = testCase.Output,
                    SubmissionResult = SubmissionResult.WrongAnswer,
                };
            }

            return new AcceptedResponse
            {
                NumberOfPassedTestCases = testcaseNumber,
                ExecutionTime = Helper.ExtractExecutionTime(runTime),
                TotalTestcases = totalTestcases,
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

        private async Task ExecuteCodeInContainer(decimal timeLimit)
        {

            string command = Helper.CreateExecuteCodeCommand(_containerId, timeLimit);

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
