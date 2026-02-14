using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;

namespace Zoco.Api.Controllers
{
    [Route("api/[controller]")]
    public class SessionLogsController : BaseController
    {
        private readonly SessionLogService _service;

        public SessionLogsController(SessionLogService service)
        {
            _service = service;
        }

        //Obtener Todos los sessionLog
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _service.GetAllLogsAsync();
            return Success(logs, "Logs obtenidos correctamente");
        }

        //Obtener un sessionLog
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _service.GetLogByIdAsync(id);

            if (log == null)
                return Failure("Log no encontrado", 404);

            return Success(log, "Log obtenido correctamente");
        }

        //Agregar Fecha fin de SessionLog
        [Authorize]
        [HttpPut("end")]
        public async Task<IActionResult> EndSession([FromQuery] int? sessionId = null)
        {
            var userId = User.GetUserId();
            var userRole = User.GetUserRole();

            SessionLog? targetSession;

            if (userRole == "Admin" && sessionId.HasValue)
            {
                // Admin puede cerrar cualquier sesión especificando el ID
                targetSession = await _service.GetSessionByIdAsync(sessionId.Value);
                if (targetSession == null)
                    return Failure("Sesión no encontrada", 404);
            }
            else
            {
                // Usuario normal cierra su sesión activa
                targetSession = await _service.GetActiveSessionByUserIdAsync(userId);
                if (targetSession == null)
                    return Failure("No hay sesión activa", 404);

                // Si alguien pasa un sessionId que no le pertenece, lo ignoramos
                if (sessionId.HasValue && targetSession.Id != sessionId.Value)
                    return Failure("No tienes permiso para cerrar esa sesión", 403);
            }

            var result = await _service.EndLogAsync(targetSession.Id);
            if (!result)
                return Failure("Error al finalizar la sesión", 400);

            return Success<string>(null, $"Sesión {(userRole == "Admin" ? "cerrada por admin" : "finalizada")} correctamente");
        }


        //Eliminar un SessionLog
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteLogAsync(id);

            if (!result)
                return Failure("Log no encontrado", 404);

            return Success<string>(null, "Log eliminado correctamente");
        }
    }

}
