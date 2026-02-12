namespace Zoco.Api.Models.DTOs
{
    // DTO genérico para respuestas de la API
    public class ApiResponse<T>
    {
        public bool Success { get; set; }         // Indica si la operación fue exitosa
        public string? Message { get; set; }      // Mensaje descriptivo
        public T? Data { get; set; }              // Datos de la respuesta 
    }
}
