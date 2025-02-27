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
        public Submit FirstSubmission { get; set; }
        public Submit SecondSubmission { get; set; }
        public decimal Similarity { get; set; }
        public int ProblemId { get; set; }
    }
}
