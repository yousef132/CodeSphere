namespace CodeSphere.Domain.Models.Entities
{
    public class Submit
    {
        public int ID { get; set; }
        public int UID { get; set; }
        public int PID { get; set; }
        public int CID { get; set; }
        public DateTime DateContest { get; set; }
        public DateTime SubmitTime { get; set; }
        public int Memory { get; set; }
        public string Result { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }
        public Problem Problem { get; set; }
        public Contest Contest { get; set; }
    }
}
