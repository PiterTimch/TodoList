using AutoMapper;
using BLL.Models.Task;
using DAL.Entities.Task;

namespace BLL.Mappers;

public class TaskMapper : Profile
{
    public TaskMapper()
    {
        CreateMap<TaskEntity, TaskItemResponseModel>();
        CreateMap<CreateTaskRequestModel, TaskEntity>();
    }
}
