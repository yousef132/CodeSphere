using CodeSphere.Domain.Premitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.DTOs
{
    public class SubmissionDTO
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public DateTime SubmissionDate { get; set; }
        public Language Language { get; set; }
    }
}
