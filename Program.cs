using Zoco.Api.Extensions;
using Zoco.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//repositorios
builder.Services.AddAllRepositories();

// Servicios
builder.Services.AddAllServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infraestructura
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Inicialización infraestructura
await app.InitializeDatabaseAsync();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
