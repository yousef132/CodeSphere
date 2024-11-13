using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Testcases.Commands.Delete
{
    public class DeleteTestcaseCommandHandler : IRequestHandler<DeleteTestcaseCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTestcaseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(DeleteTestcaseCommand request, CancellationToken cancellationToken)
        {
            var testcase = await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().GetByIdAsync(request.TestcaseId);
            if (testcase == null)
                return await Response.FailureAsync("Test case not found!", System.Net.HttpStatusCode.NotFound);

            await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().DeleteAsync(testcase);
            await _unitOfWork.CompleteAsync();

            return await Response.SuccessAsync(null, "Test case deleted successfully.", System.Net.HttpStatusCode.NoContent);
        }
    }
}
