using BLL.Models.Task;

namespace BLL.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItemResponseModel>> SearchTasksAsync(TasksSearchRequestModel request);
    Task<TaskItemResponseModel> CreateTaskAsync(CreateTaskRequestModel request);
    Task DeleteTaskAsync(long id);
    Task<TaskItemResponseModel> GetTaskByIdAsync(long id);
    Task SetTaskCompletedAsync(SetTaskCompletedRequestModel request);
}
