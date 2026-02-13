using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;
using System.Collections.Generic;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Success(users, "Usuarios obtenidos correctamente");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return Failure("Usuario no encontrado", 404);

            return Success(user, "Usuario obtenido correctamente");
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDTO dto)
        {
            var (success, error, user) =
                await _userService.CreateUserAsync(dto);

            if (!success)
                return Failure(error!, 400);

            return Success(user, "Usuario creado correctamente", 201);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDTO dto)
        {
            var (success, error) =
                await _userService.UpdateUserAsync(id, dto);

            if (!success)
                return Failure(error!, 404);

            return Success<object>(null, "", 204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, error) =
                await _userService.DeleteUserAsync(id);

            if (!success)
                return Failure(error!, 404);

            return Success<object>(null, "", 204);
        }
    }

}
