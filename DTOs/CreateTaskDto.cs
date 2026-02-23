using System.ComponentModel.DataAnnotations;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    [EnumDataType(typeof(TaskPriority))]
    public TaskPriority? Priority { get; set; }

    [Required]
    [EnumDataType(typeof(Status))]
    public Status? Status { get; set; }
    public DateTime? DueDate { get; set; }
}
