namespace CodeSphere.Domain.Models.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
        public int Ranking { get; set; }

        public ICollection<Contest> Contests { get; set; } // Contests created by this user (Setter)
        public ICollection<Problem> Problems { get; set; } // Problems set by this user (Setter)
        public ICollection<Tutorial> Tutorials { get; set; } // Tutorials created by this user (Setter)
        public ICollection<Register> Registrations { get; set; } // Contests registered by this user
        public ICollection<Submit> Submissions { get; set; } //
    }
}
