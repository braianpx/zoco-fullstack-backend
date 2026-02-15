using Zoco.Api.Extensions;
using Zoco.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

//URl api
var urls = builder.Configuration["UrlApi"];
if (!string.IsNullOrEmpty(urls)) builder.WebHost.UseUrls(urls.Split(';'));

// --- CONFIGURACIÓN DE RUTAS ---
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// --- CONFIGURACIÓN DE CORS ---
var misReglasCors = "ReglasCors";
builder.Services.AddCustomCors(builder.Configuration, misReglasCors);

//validaciones personalizadas
builder.Services.AddCustomApiBehavior();

//repositorios
builder.Services.AddAllRepositories();

//Filtros
builder.Services.AddScoped<UserAccessFilter>();

//JWT
builder.Services.AddJwtConfiguration(builder.Configuration);

//Swagger
builder.Services.AddSwaggerConfiguration();

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
app.UseCors(misReglasCors);
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
