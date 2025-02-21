using CodeSphere.Domain.Premitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions
{
    public class GetProblemSubmissionsResponse
    {
        public decimal SubmitTime { get; set; }  // run time of the submission 
        public decimal SubmitMemory { get; set; }
        public SubmissionResult Result { get; set; }
        public DateTime SubmissionDate { get; set; }
        public Language Language { get; set; }
        public int Id { get; set; }
    }
}
