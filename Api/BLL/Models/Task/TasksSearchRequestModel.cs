namespace BLL.Models.Task;

public class TasksSearchRequestModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DueDate { get; set; }
    public bool? IsCompleted { get; set; }
}
