namespace BLL.Models.Task;

public class TaskItemResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DateCreated { get; set; }
}
