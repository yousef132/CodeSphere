using AutoMapper;
using CodeSphere.Application.Features.Topic.Commands;
using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Mapping
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, CreateTopicCommandResponse>();
        }
    }
}
