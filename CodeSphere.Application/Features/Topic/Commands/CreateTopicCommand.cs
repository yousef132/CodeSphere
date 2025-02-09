using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Topic.Commands
{
    public sealed record CreateTopicCommand(
        string Name
    ) : IRequest<Response>;
}
