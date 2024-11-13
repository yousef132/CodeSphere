using CodeSphere.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeSphere.Infrastructure
{
    public class DatabaseManagementService
    {
        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                // Check if the database exists, if not, create it
                if (!context.Database.CanConnect())
                {
                    // Creates the database if it does not exist
                    context.Database.EnsureCreated();
                }

                // Apply pending migrations
                context.Database.Migrate();
            }

        }
    }
}
