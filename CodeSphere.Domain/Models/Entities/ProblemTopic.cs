namespace CodeSphere.Domain.Models.Entities
{
    public class ProblemTopic
    {
        public int PID { get; set; }
        public int TID { get; set; }

        public Problem Problem { get; set; }
        public Topic Topic { get; set; }
    }
}
