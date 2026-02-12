using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;
using System.Collections.Generic;

namespace Zoco.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // GET /api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new ApiResponse<List<UserResponseDTO>>
            {
                Success = true,
                Message = "Usuarios obtenidos correctamente",
                Data = users
            });
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuario no encontrado",
                    Data = null
                });

            return Ok(new ApiResponse<UserResponseDTO>
            {
                Success = true,
                Message = "Usuario obtenido correctamente",
                Data = user
            });
        }

        // POST /api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                return BadRequest(new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "Errores de validación",
                    Data = errors
                });
            }

            var (success, error, user) = await _userService.CreateUserAsync(dto);

            if (!success)
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = error,
                    Data = null
                });

            return CreatedAtAction(nameof(GetById), new { id = user!.Id }, new ApiResponse<UserResponseDTO>
            {
                Success = true,
                Message = "Usuario creado correctamente",
                Data = user
            });
        }

        // PUT /api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();

                return BadRequest(new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "Errores de validación",
                    Data = errors
                });
            }

            var (success, error) = await _userService.UpdateUserAsync(id, dto);

            if (!success)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = error,
                    Data = null
                });

            return NoContent(); // 204, actualización exitosa sin cuerpo
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, error) = await _userService.DeleteUserAsync(id);

            if (!success)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = error,
                    Data = null
                });

            return NoContent(); // 204, eliminado exitoso
        }
    }
}
