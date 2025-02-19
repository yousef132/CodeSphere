using System.Runtime.Serialization;

namespace CodeSphere.Domain.Premitives
{
    public enum UserStatus
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
