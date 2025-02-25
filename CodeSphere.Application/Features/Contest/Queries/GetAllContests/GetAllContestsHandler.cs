using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Queries.GetAllContests
{
    public class GetAllContestsHandler : IRequestHandler<GetAllContestsQuery, Response>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GetAllContestsHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetAllContestsQuery request, CancellationToken cancellationToken)
        {
            var contests = await unitOfWork.Repository<Domain.Models.Entities.Contest>().GetAllAsync();

            var mappedContests = mapper.Map<IReadOnlyList<GetAllContestsQueryResponse>>(contests);

            return await Response.SuccessAsync(mappedContests);

        }
    }
}
