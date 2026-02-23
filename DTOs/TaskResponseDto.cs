using TaskManagerApplication.Models;

namespace TaskManagerApplication.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority? Priority { get; set; }
        public Status? Status { get; set; }
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; } // optional, smart addition
    }
}
