using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;

namespace Zoco.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudiesController : BaseController
    {
        private readonly StudyService _service;

        public StudiesController(StudyService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();

            var studies = await _service.GetAllAsync(userId, role);

            return Success(studies, "Estudios obtenidos correctamente");
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();

            var study = await _service.GetByIdAsync(id, userId, role);

            if (study == null)
                return Failure("Estudio no encontrado", 404);

            return Success(study, "Estudio obtenido correctamente");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(StudyCreateDTO dto)
        {
            var userId = User.GetUserId();

            var result = await _service.CreateAsync(userId, dto);

            return Success(result, "Estudio creado correctamente", 201);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var currentUserId = User.GetUserId();
            var role = User.GetUserRole();

            // El service debe devolver una lista (IEnumerable o Array)
            var studies = await _service.GetByUserIdAsync(userId, currentUserId, role);

            if (studies == null || !studies.Any())
                return Success(studies, "El usuario no tiene estudios registrados", 200);

            return Success(studies, "Estudios obtenidos correctamente", 200);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudyUpdateDTO dto)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();

            var updated = await _service.UpdateAsync(id, userId, role, dto);

            if (!updated)
                return Failure("Estudio no encontrado", 404);

            return Success<object>(null, "Estudio actualizado correctamente");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole();

            var deleted = await _service.DeleteAsync(id, userId, role);

            if (!deleted)
                return Failure("Estudio no encontrado", 404);

            return Success<object>(null, "Estudio eliminado correctamente");
        }
    }

}
