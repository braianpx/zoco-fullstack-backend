using Microsoft.AspNetCore.Mvc;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;

namespace Zoco.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionLogsController : ControllerBase
    {
        private readonly SessionLogService _service;

        public SessionLogsController(SessionLogService service)
        {
            _service = service;
        }

        // GET api/sessionlogs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _service.GetAllLogsAsync();
            return Ok(new ApiResponse<List<SessionLogResponseDTO>>
            {
                Success = true,
                Message = "Logs obtenidos correctamente",
                Data = logs
            });
        }

        // GET api/sessionlogs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _service.GetLogByIdAsync(id);
            if (log == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Log no encontrado" });

            return Ok(new ApiResponse<SessionLogResponseDTO>
            {
                Success = true,
                Message = "Log obtenido correctamente",
                Data = log
            });
        }


        // PUT api/sessionlogs/{id}/end
        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndSession(int id)
        {
            var result = await _service.EndLogAsync(id);
            if (!result)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Log no encontrado" });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Logout registrado correctamente"
            });
        }

        // DELETE api/sessionlogs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteLogAsync(id);
            if (!result)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Log no encontrado" });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Log eliminado correctamente"
            });
        }
    }
}
