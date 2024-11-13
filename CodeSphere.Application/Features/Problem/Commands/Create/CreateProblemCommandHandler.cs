using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using System.Net;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public class CreateProblemCommandHandler :
        IRequestHandler<CreateProblemCommand, Response>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CreateProblemCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(CreateProblemCommand request, CancellationToken cancellationToken)
        {
            var contest = await unitOfWork.Repository<Contest>().GetByIdAsync(request.ContestId);
            if (contest == null)
                return await Response.FailureAsync("Contest Not Found!!", System.Net.HttpStatusCode.NotFound);



            var mappedProblem = mapper.Map<Domain.Models.Entities.Problem>(request);

            await unitOfWork.Repository<Domain.Models.Entities.Problem>().AddAsync(mappedProblem);
            await unitOfWork.CompleteAsync();

            var response = mapper.Map<CreateProblemCommandResponse>(mappedProblem);

            return await Response.SuccessAsync(response, "Problem added successfully", HttpStatusCode.Created);
        }
    }
}
