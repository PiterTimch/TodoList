using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BLL;
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
}
