using System.ComponentModel.DataAnnotations;

namespace BLL.Models.Task;

public class CreateTaskRequestModel
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(300)]
    public string Description { get; set; } = null!;

    public string? DueDate { get; set; }
}
