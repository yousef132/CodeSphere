using AutoMapper;
using CodeSphere.Application.Features.Authentication.Commands.Register;
using CodeSphere.Application.Features.Authentication.Queries.Login;
using CodeSphere.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Mapping
{
    public class ApplicationUserProfile : Profile
	{
        public ApplicationUserProfile()
        {
            CreateMap<RegisterCommand, ApplicationUser>();
            CreateMap<LoginQuery, ApplicationUser>();
        }
    }
}
