﻿using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CodeSphere.Application.Features.Authentication.Queries.ConfirmEmail
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, Response>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return await Response.FailureAsync("Invalid Confirmation Code!!!");

            if (user.EmailConfirmed)
                return await Response.FailureAsync(user.Email, "Email already Confirmed");

            var code = request.Code;

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return await Response.FailureAsync(user.Email, "Can't Decode the Code");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(user, );
                return await Response.SuccessAsync("Email Confirmed!!!");
            }

            var error = result.Errors.First();

            return await Response.FailureAsync(user.Email, "Can't Confirm the Email ", System.Net.HttpStatusCode.BadRequest);
        }
    }
}
