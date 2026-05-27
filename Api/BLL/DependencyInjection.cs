using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DAL;
using BLL.Interfaces;
using BLL.Services;

namespace BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }
}
