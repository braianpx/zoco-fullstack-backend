using Microsoft.AspNetCore.Mvc;
using Zoco.Api.Models.DTOs;

namespace Zoco.Api.Extensions
{
    public static class ApiBehaviorExtensions
    {
        public static IServiceCollection AddCustomApiBehavior(
            this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = new ApiResponse<List<string>>
                    {
                        Success = false,
                        Message = "Errores de validación",
                        Data = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
