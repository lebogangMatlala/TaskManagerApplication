using System.ComponentModel.DataAnnotations;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.DTOs;

public class CreateProjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    [EnumDataType(typeof(ProjectStatus))]
    public ProjectStatus? Status { get; set; }
}
