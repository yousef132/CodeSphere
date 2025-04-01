using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CodeSphere.Application.Features.User.Commnad.EditDetailsQuery
{
    public class EditUserDetailsQueryHandler : IRequestHandler<EditUserDetailsQuery, Response>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly string UserId;
        public EditUserDetailsQueryHandler(UserManager<ApplicationUser> userManager,
                                            IFileService _fileService,
                                            IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            this._fileService = _fileService;
            this.contextAccessor = contextAccessor;
            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(EditUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
                return await Response.FailureAsync(message: "User not found");

            if (request.Image != null)
                user.ImagePath = await _fileService.UploadFileAsync(request.Image, Helper.ImagesDirectory);

            user.Gender = request.Gender;
            user.Name = request.Name;

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return await Response.SuccessAsync(data: user?.ImagePath ?? "", message: "User details updated successfully");

            return await Response.FailureAsync(message: "Failed to update user details");
        }
    }

}
