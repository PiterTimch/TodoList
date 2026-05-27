using AutoMapper;
using BLL.Interfaces;
using BLL.Models.Task;
using DAL;
using DAL.Entities.Task;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class TaskService(AppDbContext context, IMapper mapper) : ITaskService
{
    public async Task<IEnumerable<TaskItemResponse>> SearchTasksAsync(TasksSearchRequestModel request)
    {
        IQueryable<TaskEntity> query = context.Tasks
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            string name = request.Name.Trim();
            query = query.Where(x => x.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            string description = request.Description.Trim();
            query = query.Where(x => x.Description.Contains(description));
        }

        if (request.DueDate.HasValue)
        {
            query = query.Where(x => x.DueDate.Value.Date == request.DueDate.Value.Date);
        }

        if (request.IsCompleted.HasValue)
        {
            query = query.Where(x => x.IsCompleted == request.IsCompleted.Value);
        }

        var entities = await query
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync();

        return mapper.Map<IEnumerable<TaskItemResponse>>(entities);
    }
}
