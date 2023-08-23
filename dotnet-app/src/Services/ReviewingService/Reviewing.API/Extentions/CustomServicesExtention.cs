namespace Reviewing.API.Extentions;

public static class CustomServicesExtention
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReviewingDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("ReviewingDb"), sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(ReviewingDbContext).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            });
        });

        return services;
    }
}