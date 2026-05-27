using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using BLL;
using BLL.Interfaces;
using BLL.Models.Task;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace TodoListApi.Tests;

public class TaskServiceTests
{
    private static ServiceProvider BuildProvider()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Configuration.AddUserSecrets("6d081823-91db-4c00-86d2-1ce41639cc02");
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        var services = new ServiceCollection();
        services.AddBusinessLogicServices(connectionString);
        return services.BuildServiceProvider();
    }

    [Fact]
    public async Task CreateTask_And_GetById_ReturnsSameData()
    {
        await using var scope = BuildProvider().CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var created = await taskService.CreateTaskAsync(new CreateTaskRequestModel
        {
            Name = "GetById Test",
            Description = "Test description",
            DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        });

        Assert.NotNull(created);
        Assert.True(created.Id > 0);

        var fetched = await taskService.GetTaskByIdAsync(created.Id);
        Assert.Equal(created.Name, fetched.Name);
        Assert.Equal(created.Description, fetched.Description);
        Assert.False(fetched.IsCompleted);

        // Cleanup
        await using var cleanupScope = BuildProvider().CreateAsyncScope();
        var db = cleanupScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entity = await db.Tasks.IgnoreQueryFilters().FirstAsync(x => x.Id == created.Id);
        db.Tasks.Remove(entity);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task SetTaskCompleted_UpdatesIsCompleted()
    {
        await using var scope = BuildProvider().CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var created = await taskService.CreateTaskAsync(new CreateTaskRequestModel
        {
            Name = "Complete Test",
            Description = "Test description",
            DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        });

        Assert.False(created.IsCompleted);

        await taskService.SetTaskCompletedAsync(new SetTaskCompletedRequestModel
        {
            Id = created.Id,
            IsCompleted = true
        });

        var fetched = await taskService.GetTaskByIdAsync(created.Id);
        Assert.True(fetched.IsCompleted);

        // Cleanup
        await using var cleanupScope = BuildProvider().CreateAsyncScope();
        var db = cleanupScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entity = await db.Tasks.IgnoreQueryFilters().FirstAsync(x => x.Id == created.Id);
        db.Tasks.Remove(entity);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task SearchTasks_NameCaseInsensitive_FindsResults()
    {
        await using var scope = BuildProvider().CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var created = await taskService.CreateTaskAsync(new CreateTaskRequestModel
        {
            Name = "CaseInsensitiveSearch",
            Description = "Test description",
            DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        });

        var results = await taskService.SearchTasksAsync(new TasksSearchRequestModel
        {
            Name = "caseinsensitivesearch"
        });

        Assert.Contains(results, x => x.Id == created.Id);

        // Cleanup
        await using var cleanupScope = BuildProvider().CreateAsyncScope();
        var db = cleanupScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entity = await db.Tasks.IgnoreQueryFilters().FirstAsync(x => x.Id == created.Id);
        db.Tasks.Remove(entity);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task SearchTasks_ByDueDateString_FindsResults()
    {
        await using var scope = BuildProvider().CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var dueDate = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd");
        var created = await taskService.CreateTaskAsync(new CreateTaskRequestModel
        {
            Name = "DueDateSearch",
            Description = "Test description",
            DueDate = dueDate
        });

        var results = await taskService.SearchTasksAsync(new TasksSearchRequestModel
        {
            DueDate = dueDate
        });

        Assert.Contains(results, x => x.Id == created.Id);

        await using var cleanupScope = BuildProvider().CreateAsyncScope();
        var db = cleanupScope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entity = await db.Tasks.IgnoreQueryFilters().FirstAsync(x => x.Id == created.Id);
        db.Tasks.Remove(entity);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteTask_MarksAsDeleted()
    {
        await using var scope = BuildProvider().CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var created = await taskService.CreateTaskAsync(new CreateTaskRequestModel
        {
            Name = "SoftDelete Test",
            Description = "Test description",
            DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        });

        await taskService.DeleteTaskAsync(created.Id);

        var entity = await db.Tasks.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == created.Id);
        Assert.NotNull(entity);
        Assert.True(entity.IsDeleted);

        // Cleanup
        db.Tasks.Remove(entity);
        await db.SaveChangesAsync();
    }
}
