using Zoco.Api.Extensions;
using Zoco.Api.Middlewares;
using Zoco.Api.Repositories;
using Zoco.Api.Services;

var builder = WebApplication.CreateBuilder(args);

//repositories
builder.Services.AddScoped<UserRepository>();

// Servicios
builder.Services.AddScoped<UserService>();
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
