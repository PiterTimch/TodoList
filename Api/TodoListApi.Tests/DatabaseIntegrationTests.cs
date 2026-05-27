using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BLL;
using BLL.Interfaces;
using BLL.Models.Task;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using DAL;

namespace TodoListApi.Tests;

public class DatabaseIntegrationTests
{
    [Fact]
    public async Task CanConnectToDatabase()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "Development"
        });
        builder.Configuration.AddUserSecrets("6d081823-91db-4c00-86d2-1ce41639cc02");
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        Assert.False(string.IsNullOrWhiteSpace(connectionString), "Connection string must be configured");

        var services = new ServiceCollection();
        services.AddBusinessLogicServices(connectionString);
        var provider = services.BuildServiceProvider();

        await using var scope = provider.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var canConnect = await db.Database.CanConnectAsync();

        Assert.True(canConnect, "Database should be reachable with the provided connection string");
    }

    [Fact]
    public async Task CreateTask_WithPastDueDate_ThrowsArgumentException()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Configuration.AddUserSecrets("6d081823-91db-4c00-86d2-1ce41639cc02");
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        var services = new ServiceCollection();
        services.AddBusinessLogicServices(connectionString);
        var provider = services.BuildServiceProvider();

        await using var scope = provider.CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

        var pastTask = new CreateTaskRequestModel
        {
            Name = "Past Task",
            Description = "Should fail",
            DueDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")
        };

        await Assert.ThrowsAsync<ArgumentException>(() => taskService.CreateTaskAsync(pastTask));
    }

    [Fact]
    public async Task CreateAndDeleteTask_Lifecycle_Succeeds()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { EnvironmentName = "Development" });
        builder.Configuration.AddUserSecrets("6d081823-91db-4c00-86d2-1ce41639cc02");
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        var services = new ServiceCollection();
        services.AddBusinessLogicServices(connectionString);
        var provider = services.BuildServiceProvider();

        await using var scope = provider.CreateAsyncScope();
        var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var newTask = new CreateTaskRequestModel
        {
            Name = "Integration Test Task",
            Description = "Testing Lifecycle",
            DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd")
        };

        var created = await taskService.CreateTaskAsync(newTask);
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(newTask.Name, created.Name);

        await taskService.DeleteTaskAsync(created.Id);

        var dbEntity = await db.Tasks.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == created.Id);
        Assert.NotNull(dbEntity);
        Assert.True(dbEntity.IsDeleted);

        db.Tasks.Remove(dbEntity);
        await db.SaveChangesAsync();
    }
}
