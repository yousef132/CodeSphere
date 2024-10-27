namespace CodeSphere.Domain.Models.Entities
{
    public class ProblemImage
    {
        public int ID { get; set; }
        public int PID { get; set; }
        public string Image { get; set; }

        public Problem Problem { get; set; }
    }
}
