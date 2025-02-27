using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Requests
{
    public class CodeSimilarityRequest
    {
        public string Code1 { get; set; }
        public string Code2 { get; set; }
    }
}
