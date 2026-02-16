using Microsoft.EntityFrameworkCore;
using Zoco.Api.Models;
using Zoco.Api.Data;
using BCrypt.Net;

namespace Zoco.Api.Data.Seed
{

    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(AppDbContext context, IConfiguration config)
        {
            // CREAR ROLES

            if (!await context.Roles.AnyAsync())
            {
                var roles = new[]
                {
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            };

                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            // OBTENER ROL ADMIN REAL DESDE DB
            var adminRole = await context.Roles
                .FirstAsync(r => r.Name == "Admin");

            // CREAR ADMIN SOLO SI NO EXISTE
            
            if (!await context.Users.AnyAsync(u => u.RoleId == adminRole.Id))
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

                if (await context.Users.FirstAsync(u => u.Email == email) == null)
                {
                    var admin = new User
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        RoleId = adminRole.Id,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                    };

                    context.Users.Add(admin);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
