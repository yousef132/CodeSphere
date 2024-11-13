using CodeSphere.Application.Helpers;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<Role> CreateRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            throw new InvalidOperationException("Role already exists.");
        }

        var identityRole = new IdentityRole(roleName);
        await _roleManager.CreateAsync(identityRole);

        var role = new Role
        {
            Id = await GetNextRoleIdAsync(),
            Name = identityRole.Name
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return role;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync(); // Make sure this returns your custom Role
    }

    public async Task<Role> GetRoleByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    private async Task<int> GetNextRoleIdAsync()
    {
        return await _context.Roles.AnyAsync()
            ? await _context.Roles.MaxAsync(r => r.Id) + 1
            : 1;
    }
}
