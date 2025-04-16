using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Command.Update
{
    public class UpdateContestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateContestCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Response> Handle(UpdateContestCommand request, CancellationToken cancellationToken)
        {
            var contestRepo = _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Contest>();
            var existingContest = await contestRepo.GetByIdAsync(request.Id);

            if (existingContest == null)
                return await Response.FailureAsync("Contest not found", System.Net.HttpStatusCode.NotFound);

            _mapper.Map(request, existingContest);

            await contestRepo.UpdateAsync(existingContest);
            await _unitOfWork.CompleteAsync();

            var result = new
            {
                existingContest.Name,
                existingContest.StartDate,
                existingContest.EndDate,
                existingContest.Duration,
            };

            return await Response.SuccessAsync(result, "Contest updated successfully", System.Net.HttpStatusCode.OK);
        }
    }
}
