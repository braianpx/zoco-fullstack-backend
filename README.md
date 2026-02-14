# Zoco Fullstack Backend

Backend desarrollado en **ASP.NET Core (.NET 8)** con autenticación JWT y Entity Framework Core.

🔗 Repositorio:  
https://github.com/braianpx/zoco-fullstack-backend.git

---

# 🚀 Cómo ejecutar el proyecto localmente

## 1️⃣ Requisitos

Antes de ejecutar el proyecto, asegúrese de tener instalado:

- ✅ .NET 8 SDK  
  https://dotnet.microsoft.com/download

- ✅ SQL Server (Local o Express)
  - SQL Server Express
  - SQL Server LocalDB
  - SQL Server Developer Edition

Verificar instalación de .NET:

```bash
dotnet --version
```

---

## 2️⃣ Clonar el repositorio

```bash
git clone https://github.com/braianpx/zoco-fullstack-backend.git
cd zoco-fullstack-backend
```

---

## 3️⃣ Restaurar dependencias

El proyecto contiene los paquetes NuGet necesarios.

```bash
dotnet restore
```

---

## 4️⃣ Configurar la base de datos

El repositorio incluye un `appsettings.json` ya configurado.

Solo es necesario modificar la cadena de conexión:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=ZocoDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Reemplazar `TU_SERVIDOR` por su instancia local.

Ejemplo para LocalDB:

```json
"Server=(localdb)\\MSSQLLocalDB;"
```

---

## 5️⃣ Ejecutar el proyecto

```bash
dotnet run
```

La aplicación:

- Ejecuta automáticamente las migraciones
- Crea la base de datos si no existe
- Crea los roles necesarios
- Crea el usuario administrador por defecto

No es necesario ejecutar:

```bash
dotnet ef database update
```

---

# 🔐 Usuario Administrador por Defecto

El sistema crea automáticamente un usuario administrador si no existe.

Credenciales:

```
Email: admin@zoco.com
Password: Admin123
```

Estas credenciales están definidas en `appsettings.json` en la sección:

```json
"AdminUser"
```

---

# 🧱 Características Técnicas

- Autenticación con JWT
- Entity Framework Core con migraciones automáticas
- Modelo `Role` para gestión de roles
- Modelo `SessionLogs` para registro de sesiones
- Método estático `Response` para estandarizar respuestas
- Arquitectura organizada y desacoplada

---

# ✅ Ejecución rápida

```bash
git clone https://github.com/braianpx/zoco-fullstack-backend.git
cd zoco-fullstack-backend
dotnet restore
dotnet run
```

Configurar únicamente la conexión a SQL Server en `appsettings.json`.
