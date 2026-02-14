using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;

namespace Zoco.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        //Obtener Todos los usuarios
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Success(users, "Usuarios obtenidos correctamente");
        }

        //Obtener un usuario
        [ServiceFilter(typeof(UserAccessFilter))]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();

            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return Failure("Usuario no encontrado", 404);

            return Success(user, "Usuario obtenido correctamente");
        }

        //Crear Usuario
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDTO dto)
        {
            var (success, error, user) =
                await _userService.CreateUserAsync(dto);

            if (!success)
                return Failure(error!, 400);

            return Success(user, "Usuario creado correctamente", 201);
        }

        //Modificar Usuario
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDTO dto)
        {
            var userRole = User.GetUserRole();

            if (userRole == "User") dto.RoleId = 2;

            var (success, error) =
                await _userService.UpdateUserAsync(id, dto);

            if (!success)
                return Failure(error!, 404);

            return Success<object>(null, "", 204);
        }

        //Borrar usuario
        [ServiceFilter(typeof(UserAccessFilter))]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userRole = User.GetUserRole();
            var userId = User.GetUserId();

            var (success, error) =
                await _userService.DeleteUserAsync(id);

            if (!success)
                return Failure(error!, 404);

            return Success<object>(null, "", 204);
        }
    }

}
