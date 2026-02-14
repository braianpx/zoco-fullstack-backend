using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;

namespace Zoco.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return Failure(result.Message!, 401);

            return Success(result.Data, result.Message!);
        }

        [Authorize] // Esta ruta requiere un token valido
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.GetUserId(); // solo funciona para rutas con Authorize

            var result = await _authService.LogoutAsync(userId);

            if (!result.Success)
                return Failure(result.Message!);

            return Success<object>(null, result.Message!);
        }
    }

}
