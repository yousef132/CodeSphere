using AutoMapper;
using CodeSphere.Domain.DTOs;
using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Mapping
{
    public class PlagiarismProfile : Profile
    {
        public PlagiarismProfile()
        {
            CreateMap<Submit, SubmissionDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language));

        }
    }
}
