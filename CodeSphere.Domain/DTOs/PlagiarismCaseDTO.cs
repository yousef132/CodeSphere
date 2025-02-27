using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.DTOs
{
    public class PlagiarismCaseDTO
    {
        public KeyValuePair<Submit, Submit> Submissions { get; set; }
        public decimal Similarity { get; set; }
        public int ProblemId { get; set; }
    }
}
