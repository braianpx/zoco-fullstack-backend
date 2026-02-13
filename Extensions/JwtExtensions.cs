using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Zoco.Api.Helpers;
using Zoco.Api.Models.DTOs;

namespace Zoco.Api.Extensions
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtSettings");

            //  Bind + Validación fuerte al iniciar
            services.AddOptions<JwtSettings>()
                .Bind(jwtSection)
                .Validate(settings =>
                    !string.IsNullOrWhiteSpace(settings.Key) &&
                    !string.IsNullOrWhiteSpace(settings.Issuer) &&
                    !string.IsNullOrWhiteSpace(settings.Audience),
                    "JwtSettings configuration is invalid.")
                .ValidateOnStart();

            // Obtener valores directamente del IConfiguration
            var key = jwtSection["Key"]
                ?? throw new InvalidOperationException("JwtSettings:Key is missing.");

            var issuer = jwtSection["Issuer"]
                ?? throw new InvalidOperationException("JwtSettings:Issuer is missing.");

            var audience = jwtSection["Audience"]
                ?? throw new InvalidOperationException("JwtSettings:Audience is missing.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = issuer,
                        ValidAudience = audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key)),

                        ClockSkew = TimeSpan.Zero
                    };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var response = new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Debe proporcionar un token válido para acceder a este recurso.",
                            Data = null
                        };

                        var json = JsonSerializer.Serialize(response);

                        return context.Response.WriteAsync(json);
                    },

                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        var response = new ApiResponse<object>
                        {
                            Success = false,
                            Message = "No tiene permisos suficientes para acceder a este recurso.",
                            Data = null
                        };

                        var json = JsonSerializer.Serialize(response);

                        return context.Response.WriteAsync(json);
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole("Admin"));
            });

            return services;
        }
    }
}
