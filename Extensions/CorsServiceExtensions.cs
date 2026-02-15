namespace Zoco.Api.Extensions
{
    public static class CorsServiceExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services,
            IConfiguration configuration, string policyName)
        {
            var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

            return services.AddCors(options =>
            {
                options.AddPolicy(name: policyName,
                    policy =>
                    {
                        if (allowedOrigins != null && allowedOrigins.Any())
                        {
                            policy.WithOrigins(allowedOrigins) // Carga el array del JSON
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials(); // Agrega esto si usas cookies/Identity
                        }
                    });
            });
        }
    }
}
