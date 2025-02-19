using System.Runtime.Serialization;

namespace CodeSphere.Domain.Premitives
{
    public enum ContestStatus
    {
        [EnumMember(Value = "Upcoming")]
        Upcoming,
        [EnumMember(Value = "Running")]
        Running,
        [EnumMember(Value = "Ended")]
        Ended
    }


}
