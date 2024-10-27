namespace CodeSphere.Domain.Models.Entities
{
    public class Contest
    {
        public int ID { get; set; }
        public int SetterID { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public User Setter { get; set; }
        public ICollection<Register> Registrations { get; set; }
        public ICollection<Problem> Problems { get; set; }
        public ICollection<Submit> Submissions { get; set; }
    }
}
