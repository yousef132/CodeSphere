using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllQueryResponse
    {
        public string Name { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public List<string> Catigories { get; set; } = new List<string>();
        public bool IsSolved { get; set; }
    }
}
