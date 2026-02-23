namespace TaskManagerApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        // optional convenience property
        public string FullName => $"{Name} {Surname}".Trim();

        public List<Project> projects { get; set; }
    }
}
