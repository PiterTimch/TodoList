using BLL.Models.Task;

namespace BLL.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItemResponse>> SearchTasksAsync(TasksSearchRequestModel request);
    Task<TaskItemResponse> CreateTaskAsync(CreateTaskRequestModel request);
    Task DeleteTaskAsync(long id);
    Task<TaskItemResponse> GetTaskByIdAsync(long id);
}
