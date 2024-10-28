using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

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
            var mappedProblem = mapper.Map<Domain.Models.Entities.Problem>(request);

            await unitOfWork.Repository<Domain.Models.Entities.Problem>().AddAsync(mappedProblem);
            await unitOfWork.CompleteAsync();

            return await Response.SuccessAsync(mappedProblem, "Product added successfully");
        }
    }
}
