namespace TaskManagerApplication.Models
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public enum Status
    {
        Pending,
        Done,
        Open,
        InProgress
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority? Priority { get; set; }
        public Status? Status { get; set; }
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
