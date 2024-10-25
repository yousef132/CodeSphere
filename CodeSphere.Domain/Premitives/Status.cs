using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Premitives
{
	public enum Status
	{
		[EnumMember(Value = "UnRanked")]
		UnRanked,

		[EnumMember(Value = "Newbie")]
		Newbie,
		[EnumMember(Value = "Pupil")]

		Pupil,
		[EnumMember(Value = "Specialist")]

		Specialist,

		[EnumMember(Value = "Expert")]
		Expert,
		[EnumMember(Value = "Candidate Master")]

		Candidate_Master,
		[EnumMember(Value = "Master")]

		Master,
		[EnumMember(Value = "International Master")]

		International_Master

	}
}
