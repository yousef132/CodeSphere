using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommand : IRequest<Response>
    {
        public List<string> TestCases { get; set; }

        public string UserId { get; set; }

        public int ProblemId { get; set; }
    }

    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, Response>
    {
        private readonly IUnitOfWork unitOfWork;

        public RunCodeCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(RunCodeCommand request, CancellationToken cancellationToken)
        {
            var problem = await unitOfWork.Repository<Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem Not Found");


            return await Response.SuccessAsync("success for now");
        }
    }
}
