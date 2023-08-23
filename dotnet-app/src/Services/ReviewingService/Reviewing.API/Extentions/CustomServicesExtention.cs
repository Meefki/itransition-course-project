using Comments.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Comments.API.Extentions;

public static class CustomServicesExtention
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CommentDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("CommentDB"), sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(CommentDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });
        });

        return services;
    }
}