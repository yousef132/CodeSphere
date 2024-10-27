namespace CodeSphere.Domain.Models.Entities
{
    public class Tutorial
    {
        public int ID { get; set; }
        public int SetterID { get; set; }
        public int PID { get; set; }
        public string Content { get; set; }

        public User Setter { get; set; }
        public Problem Problem { get; set; }
        public ICollection<TutorialImage> Images { get; set; }
    }
}
