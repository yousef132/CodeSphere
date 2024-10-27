namespace CodeSphere.Domain.Models.Entities
{
    public class Problem
    {
        public int ID { get; set; }
        public int SetterID { get; set; }
        public int CID { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Description { get; set; }

        public User Setter { get; set; }
        public Contest Contest { get; set; }
        public ICollection<ProblemImage> Images { get; set; }
        public ICollection<Testcase> Testcases { get; set; }
        public ICollection<Tutorial> Tutorials { get; set; }
        public ICollection<ProblemTopic> ProblemTopics { get; set; }
        public ICollection<Submit> Submissions { get; set; }
    }
}
