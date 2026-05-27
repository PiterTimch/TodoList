using System.ComponentModel.DataAnnotations;

namespace BLL.Models.Task;

public class SetTaskCompletedRequestModel
{
    [Required]
    public long Id { get; set; }

    [Required]
    public bool IsCompleted { get; set; }
}
