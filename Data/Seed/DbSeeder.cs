using Microsoft.EntityFrameworkCore;
using Zoco.Api.Models;
using Zoco.Api.Data;
using BCrypt.Net;

namespace Zoco.Api.Data.Seed
{
    /// <summary>
    /// Inicializa roles y usuario admin por defecto usando BCrypt para contraseñas.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(AppDbContext context, IConfiguration config)
        {
            // ========================
            // CREAR ROLES
            // ========================
            if (!await context.Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new Role { Id = 1, Name = "Admin" },
                    new Role { Id = 2, Name = "User" }
                };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            // ========================
            // CREAR ADMIN
            // ========================
            if (!await context.Users.AnyAsync(u => u.RoleId == 1))
            {
                var adminConfig = config.GetSection("AdminUser");
                var email = adminConfig.GetValue<string>("Email")
                    ?? throw new InvalidOperationException("Email del admin no configurado");
                var password = adminConfig.GetValue<string>("Password")
                    ?? throw new InvalidOperationException("Password del admin no configurado");
                var firstName = adminConfig.GetValue<string>("FirstName")
                    ?? throw new InvalidOperationException("FirstName del admin no configurado");
                var lastName = adminConfig.GetValue<string>("LastName")
                    ?? throw new InvalidOperationException("LastName del admin no configurado");

                var admin = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    RoleId = 1,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
