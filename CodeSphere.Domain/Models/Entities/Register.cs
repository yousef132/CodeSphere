namespace CodeSphere.Domain.Models.Entities
{
    public class Register
    {
        public int UID { get; set; }
        public int CID { get; set; }

        public User User { get; set; }
        public Contest Contest { get; set; }
    }
}
