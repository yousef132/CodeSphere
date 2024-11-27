using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.ElasticSearchResponses;
using MediatR;
using System.Net;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public class CreateProblemCommandHandler :
        IRequestHandler<CreateProblemCommand, Response>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IElasticSearchRepository elasticSearchRepository;

        public CreateProblemCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IElasticSearchRepository elasticSearchRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.elasticSearchRepository = elasticSearchRepository;
        }
        public async Task<Response> Handle(CreateProblemCommand request, CancellationToken cancellationToken)
        {
            var contest = await unitOfWork.Repository<Contest>().GetByIdAsync(request.ContestId);
            if (contest == null)
                return await Response.FailureAsync("Contest Not Found!!", System.Net.HttpStatusCode.NotFound);



            var mappedProblem = mapper.Map<Domain.Models.Entities.Problem>(request);

            await unitOfWork.Repository<Domain.Models.Entities.Problem>().AddAsync(mappedProblem);
            await unitOfWork.CompleteAsync();

            var document = new ProblemDocument
            {
                Difficulty = mappedProblem.Difficulty,
                Id = mappedProblem.Id,
                Name = mappedProblem.Name,
                Topics = new List<string>() // TODO : Will be  Filled with problem topics
            };

            var result = await elasticSearchRepository.IndexDocumentAsync(document, ElasticSearchIndexes.Problems);

            if (!result)
                return await Response.FailureAsync("failed to index problem", HttpStatusCode.Created);

            var response = mapper.Map<CreateProblemCommandResponse>(mappedProblem);

            return await Response.SuccessAsync(response, "Problem added successfully", HttpStatusCode.Created);
        }
    }
}
