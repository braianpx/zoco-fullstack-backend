using System.Security.Claims;
using Zoco.Api.Models;

namespace Zoco.Api.Services.Interfaces
{
    public interface IJwtService
    {
        public (string Token, DateTime Expiration) GenerateToken(User user, string roleName);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
