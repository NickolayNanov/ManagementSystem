using ManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem
{
    public static class Extensions
    {
        public static async Task MigrateDatabase(this IServiceProvider serviceProvider)
        {
            AppDbContext context;

            using (var scope = serviceProvider.CreateScope())
            {
                context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }

            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }

            // seed
        }
    }
}
