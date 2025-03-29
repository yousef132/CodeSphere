using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager);
    }
}
