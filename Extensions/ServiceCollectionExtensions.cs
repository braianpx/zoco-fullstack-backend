using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Zoco.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            // Obtenemos el assembly actual
            var assembly = Assembly.GetExecutingAssembly();

            // Buscamos todas las clases que terminen en "Service"
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"));

            foreach (var type in serviceTypes)
            {
                // Registra cada clase como Scoped
                services.AddScoped(type);
            }

            return services;
        }

        // Lo mismo que service solo que para los repostorios
        public static IServiceCollection AddAllRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"));

            foreach (var type in repositoryTypes)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}
