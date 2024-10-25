using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Models.Identity
{
	public class ApplicationUser : IdentityUser
	{
        public short Rate { get; set; } = 0;  // short = int 
        public Status Status { get; set; } = Status.UnRanked;

    }
}
