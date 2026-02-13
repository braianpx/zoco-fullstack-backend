using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _service.GetAllLogsAsync();
            return Success(logs, "Logs obtenidos correctamente");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _service.GetLogByIdAsync(id);

            if (log == null)
                return Failure("Log no encontrado", 404);

            return Success(log, "Log obtenido correctamente");
        }

        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndSession(int id)
        {
            var result = await _service.EndLogAsync(id);

            if (!result)
                return Failure("Log no encontrado", 404);

            return Success<string>(null, "Logout registrado correctamente");
        }

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
