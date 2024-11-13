using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Helpers
{
    public interface IRoleService
    {
        Task<Role> CreateRoleAsync(string roleName);
        Task<Role> GetRoleByIdAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
