using BLL.Models.Task;

namespace BLL.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItemResponse>> SearchTasksAsync(TasksSearchRequestModel request);
}
