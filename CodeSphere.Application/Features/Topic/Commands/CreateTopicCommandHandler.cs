using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Topic.Commands
{
    public class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Response>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public CreateTopicCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = new Domain.Models.Entities.Topic
            {
                Name = request.Name
            };

            await _unitOfWork.Repository<Domain.Models.Entities.Topic>().AddAsync(topic);
            await _unitOfWork.CompleteAsync();

            var mappedTopic = _mapper.Map<CreateTopicCommandResponse>(topic); 

            return await Response.SuccessAsync(mappedTopic, "Topic Created Successfully !!");
        }
    }
}
