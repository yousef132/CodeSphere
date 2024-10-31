using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Abstractions.Services
{
	public interface IAuthService
	{
		Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager);
	}
}
