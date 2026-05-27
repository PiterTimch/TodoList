using AutoMapper;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models.Task;
using DAL;
using DAL.Entities.Task;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class TaskService(AppDbContext context, IMapper mapper) : ITaskService
{
    public async Task<IEnumerable<TaskItemResponseModel>> SearchTasksAsync(TasksSearchRequestModel request)
    {
        IQueryable<TaskEntity> query = context.Tasks
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            string name = request.Name.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            string description = request.Description.Trim().ToLower();
            query = query.Where(x => x.Description.ToLower().Contains(description));
        }

        DateTime? dueDate = DateStringParser.ParseToUtcDate(request.DueDate);
        if (dueDate.HasValue)
        {
            DateTime searchDate = dueDate.Value.Date;
            query = query.Where(x => x.DueDate.HasValue && x.DueDate.Value.Date == searchDate);
        }

        if (request.IsCompleted.HasValue)
        {
            query = query.Where(x => x.IsCompleted == request.IsCompleted.Value);
        }

        var entities = await query
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync();

        return mapper.Map<IEnumerable<TaskItemResponseModel>>(entities);
    }

    public async Task<TaskItemResponseModel> CreateTaskAsync(CreateTaskRequestModel request)
    {
        DateTime? dueDate = DateStringParser.ParseToUtcDate(request.DueDate);
        if (dueDate.HasValue && dueDate.Value.Date < DateTime.UtcNow.Date)
        {
            throw new ArgumentException("Due date cannot be in the past.");
        }

        var entity = mapper.Map<TaskEntity>(request);
        entity.DueDate = dueDate;
        entity.DateCreated = DateTime.UtcNow;
        entity.IsDeleted = false;
        entity.IsCompleted = false;

        context.Tasks.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<TaskItemResponseModel>(entity);
    }

    public async Task DeleteTaskAsync(long id)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} does not exist.");
        }

        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<TaskItemResponseModel> GetTaskByIdAsync(long id)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} does not exist.");
        }
        return mapper.Map<TaskItemResponseModel>(entity);
    }

    public async Task SetTaskCompletedAsync(SetTaskCompletedRequestModel request)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} does not exist.");
        }
        entity.IsCompleted = request.IsCompleted;
        await context.SaveChangesAsync();
    }

}
