namespace TaskManagerApplication.Models
{
    public enum ProjectStatus
    {
        NotStarted,
        Active,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectStatus? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }

        public List<TaskItem> Tasks { get; set; }
    }
}
