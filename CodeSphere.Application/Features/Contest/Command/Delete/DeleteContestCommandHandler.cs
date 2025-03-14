using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.DTOs;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Command.Delete
{
    public class DeleteContestCommandHandler : IRequestHandler<DeleteContestCommand, Response>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteContestCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(DeleteContestCommand request, CancellationToken cancellationToken)
        {
            var contest = await unitOfWork.Repository<Domain.Models.Entities.Contest>().GetByIdAsync(request.Id);
            if (contest == null)
            {
                return await Response.FailureAsync("Contest not found", System.Net.HttpStatusCode.NotFound);
            }

            await unitOfWork.Repository<Domain.Models.Entities.Contest>().DeleteAsync(contest);
            await unitOfWork.CompleteAsync();

            var responseDto = new ContestResponseDto
            {
                Id = contest.Id,
                Name = contest.Name
            };

            return await Response.SuccessAsync(responseDto, "Contest deleted successfully", System.Net.HttpStatusCode.OK);
        }
    }
}
