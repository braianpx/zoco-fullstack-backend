using Microsoft.EntityFrameworkCore;
using Zoco.Api.Data;

namespace Zoco.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();

                logger.LogInformation("---- Base de datos migrada y conectada correctamente. ---");
            }
            catch (Exception ERR)
            {
                logger.LogError("--- Error al inicializar la base de datos : " + ERR.ToString());
                throw;
            }
        }
    }
}
