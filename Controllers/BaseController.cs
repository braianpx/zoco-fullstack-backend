using Microsoft.AspNetCore.Mvc;
using Zoco.Api.Models.DTOs;

namespace Zoco.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(
            T? data,
            string message,
            int statusCode = 200)
        {
            if (statusCode == 204)
                return NoContent();

            return StatusCode(statusCode, new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        protected IActionResult Failure(
            string message,
            int statusCode = 400)
        {
            return StatusCode(statusCode, new ApiResponse<object>
            {
                Success = false,
                Message = message,
                Data = null
            });
        }
    }
}
