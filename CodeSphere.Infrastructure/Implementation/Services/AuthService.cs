using CodeSphere.Application.Helpers;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;

        public AuthService(IConfiguration configuration, IOptions<JwtOptions> jwtOptions)
        {
            this._jwtOptions = jwtOptions.Value;
        }

        public async Task<string> CreateTokenAsync(ApplicationUser User,
                                                    UserManager<ApplicationUser> userManager
                                                    )
        {
            if (User == null || string.IsNullOrEmpty(User.Id))
            {
                throw new ArgumentNullException("User or User.Id is null.");
            }

            //1. Privat Claims[User - Defined]
            var AuthClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.Id),
                new Claim(ClaimTypes.Name, User.UserName),
                new Claim("ImagePath", User.ImagePath??"")
            };

            var UserRoles = await userManager.GetRolesAsync(User);
            foreach (var role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 2. Implement Key
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key ?? string.Empty));

            // 3. Register Claim [Implement Object Which will Create Token]
            var token = new JwtSecurityToken(

            audience: _jwtOptions.Audience,
            issuer: _jwtOptions.Issure,
            expires: DateTime.Now.AddDays(_jwtOptions.DurationInDays),
            claims: AuthClaims,
            signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
            );

            // 4. Return token 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
