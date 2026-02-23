using System.ComponentModel.DataAnnotations;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.DTOs;

public class UpdateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    [EnumDataType(typeof(TaskPriority))]
    public TaskPriority? Priority { get; set; }

    [EnumDataType(typeof(Status))]
    public Status? Status { get; set; }
    public DateTime? DueDate { get; set; }
}
