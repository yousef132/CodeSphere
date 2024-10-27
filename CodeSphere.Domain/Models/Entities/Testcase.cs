namespace CodeSphere.Domain.Models.Entities
{
    public class Testcase
    {
        public int ID { get; set; }
        public int PID { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

        public Problem Problem { get; set; }
    }
}
