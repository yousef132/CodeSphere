using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
	public class GetByIdQueryResponse
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public List<TestCasesDto> TasteCases { get; set; }
		public List<TopicDto> Topics { get; set; }

        public decimal Accepted { get; set; }
        public decimal Submissions { get; set; }
		public decimal AcceptanceRate => Submissions > 0 ? Accepted / Submissions * 100 : 0;

		public bool IsSolved { get; set; }

	}
}
