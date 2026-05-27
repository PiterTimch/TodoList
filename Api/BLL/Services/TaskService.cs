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

    public async Task<TaskItemResponse> CreateTaskAsync(CreateTaskRequestModel request)
    {
        if (request.DueDate.HasValue && request.DueDate.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("Due date cannot be in the past.");
        }

        var entity = mapper.Map<TaskEntity>(request);
        entity.DateCreated = DateTime.UtcNow;
        entity.IsDeleted = false;
        entity.IsCompleted = false;

        context.Tasks.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<TaskItemResponse>(entity);
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

    public async Task<TaskItemResponse> GetTaskByIdAsync(long id)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} does not exist.");
        }
        return mapper.Map<TaskItemResponse>(entity);
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
