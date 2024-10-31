using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Infrastructure.Implementation.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
        {
			_configuration = configuration;
		}

        public async Task<string> CreateTokenAsync(ApplicationUser User, UserManager<ApplicationUser> userManager)
		{
			if (User == null || string.IsNullOrEmpty(User.Id))
			{
				throw new ArgumentNullException("User or User.Id is null.");
			}

			//1. Privat Claims[User - Defined]
			var AuthClaims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub, User.Id)
				//new Claim(ClaimTypes.Sid, User.Id)
			};

			var UserRoles = await userManager.GetRolesAsync(User);
			foreach(var role in UserRoles)
			{
				AuthClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			// 2. Implement Key
			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));

			// 3. Register Claim [Implement Object Which will Create Token]
			var token = new JwtSecurityToken(

			audience: _configuration["JWT:ValidAudience"],
			issuer: _configuration["JWT:ValidIssure"],
			expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"] ?? string.Empty)),
			claims: AuthClaims,
			signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
			);

			// 4. Return token 
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
