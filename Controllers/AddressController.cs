using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zoco.Api.Controllers;
using Zoco.Api.Models;
using Zoco.Api.Models.DTOs;
using Zoco.Api.Services;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : BaseController
{
    private readonly AddressService _service;

    public AddressesController(AddressService service)
    {
        _service = service;
    }

    // Obtener todos (solo admin)
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();

        var addresses = await _service.GetAllAsync(userId,role);
        return Success(addresses, "Direcciones obtenidas correctamente");
    }

    // Obtener por id
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();

        var address = await _service.GetByIdAsync(id, userId, role);

        if (address == null)
            return Failure("Dirección no encontrada", 404);

        return Success(address, "Dirección obtenida correctamente");
    }

    // Crear
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddressCreateDTO dto)
    {
        var userId = User.GetUserId();

        var result = await _service.CreateAsync(userId, dto);

        return Success(result, "Dirección creada correctamente", 201);
    }

    // Actualizar
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AddressUpdateDTO dto)
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();

        var updated = await _service.UpdateAsync(id, userId, role, dto);

        if (!updated)
            return Failure("Dirección no encontrada", 404);

        return Success<object>(null, "Dirección actualizada correctamente");
    }

    // Eliminar
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetUserId();
        var role = User.GetUserRole();

        var deleted = await _service.DeleteAsync(id, userId, role);

        if (!deleted)
            return Failure("Dirección no encontrada", 404);

        return Success<object>(null, "Dirección eliminada correctamente");
    }
}
