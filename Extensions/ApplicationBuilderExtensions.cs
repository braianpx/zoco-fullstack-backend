using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zoco.Api.Data;
using Zoco.Api.Data.Seed;

namespace Zoco.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        // Inicializa la base de datos aplicando migraciones y creando roles/admin por defecto.
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            try
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Aplica migraciones pendientes
                await context.Database.MigrateAsync();
                logger.LogInformation("---- Base de datos migrada correctamente. ---");

                // Seed de roles y admin
                await DbSeeder.SeedRolesAndAdminAsync(context, config);
                logger.LogInformation("---- Roles y usuario admin inicializados correctamente. ---");
            }
            catch (Exception ex)
            {
                logger.LogError("--- Error al inicializar la base de datos: " + ex);
                throw;
            }
        }
    }
}
