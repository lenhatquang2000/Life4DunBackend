using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Life4DunBackend.Infrastructure.Data.Seeders;

/// <summary>
/// Extension methods để sử dụng DatabaseSeeder trong Program.cs
/// </summary>
public static class SeederExtensions
{
    /// <summary>
    /// Seed dữ liệu mẫu vào database khi ứng dụng khởi động
    /// </summary>
    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            try
            {
                var context = scope.ServiceProvider.GetRequiredService<GameDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();
                
                var seeder = new DatabaseSeeder(context, logger);
                await seeder.SeedAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeeder>>();
                logger.LogError($"An error occurred while seeding the database: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Đăng ký DatabaseSeeder trong DI container
    /// </summary>
    public static IServiceCollection AddDatabaseSeeder(this IServiceCollection services)
    {
        services.AddScoped<DatabaseSeeder>();
        return services;
    }
}
