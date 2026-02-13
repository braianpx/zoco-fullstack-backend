using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Zoco.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Registrar automáticamente todos los Services
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            // Obtener el assembly actual
            var assembly = Assembly.GetExecutingAssembly();

            // Buscar todas las clases concretas que terminen en "Service"
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.Name.EndsWith("Service"));

            foreach (var implementationType in serviceTypes)
            {
                // Buscar si existe una interfaz que siga la convención:
                // I + NombreClase  (Ej: IJwtService → JwtService)
                var interfaceType = implementationType
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    // Registrar interfaz → implementación
                    services.AddScoped(interfaceType, implementationType);
                }
                else
                {
                    // Si no tiene interfaz, registrar solo la clase concreta
                    services.AddScoped(implementationType);
                }
            }

            return services;
        }

        // Registrar automáticamente todos los Repositories
        public static IServiceCollection AddAllRepositories(this IServiceCollection services)
        {
            // Obtener el assembly actual
            var assembly = Assembly.GetExecutingAssembly();

            // Buscar todas las clases concretas que terminen en "Repository"
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.Name.EndsWith("Repository"));

            foreach (var implementationType in repositoryTypes)
            {
                // Buscar si existe una interfaz que siga la convención:
                // I + NombreClase  (Ej: IUserRepository → UserRepository)
                var interfaceType = implementationType
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    // Registrar interfaz → implementación
                    services.AddScoped(interfaceType, implementationType);
                }
                else
                {
                    // Si no tiene interfaz, registrar solo la clase concreta
                    services.AddScoped(implementationType);
                }
            }

            return services;
        }
    }
}
