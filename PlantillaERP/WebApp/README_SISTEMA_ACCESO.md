# 🔐 RESUMEN: Sistema de Gestión de Acceso a Páginas

## ✅ Completado

Tu proyecto ERP ahora tiene un **sistema completo de autenticación y autorización** usando `UserRoles.Identity`. El proyecto compila exitosamente sin errores.

---

## 📦 Arquitectura Implementada

### 1. **Modelos de Base de Datos**

| Modelo | Ubicación | Descripción |
|--------|-----------|-------------|
| `ModulePermission` | `UserRoles.Identity/Models/` | Define permisos por módulo (Ver, Crear, Editar, Eliminar) |
| `RolePermission` | `UserRoles.Identity/Models/` | Vincula roles con permisos |
| `Users` | `UserRoles.Identity/Models/` | Extiende IdentityUser (ya existente) |
| `AppDbContext` | `UserRoles.Identity/Data/` | Actualizado con DbSets para permisos |

### 2. **Servicio de Permisos**

```csharp
// Ubicación: UserRoles.Identity/Services/PermissionService.cs
public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(string userId, string moduleName, string permissionName);
    Task<bool> UserHasModuleAccessAsync(string userId, string moduleName);
    Task<List<string>> GetUserModulesAsync(string userId);
    Task<List<string>> GetUserPermissionsAsync(string userId, string moduleName);
    Task AssignPermissionToRoleAsync(string roleId, int modulePermissionId);
    Task RemovePermissionFromRoleAsync(string roleId, int modulePermissionId);
    Task<List<ModulePermission>> GetModulePermissionsAsync(string moduleName);
}
```

### 3. **Constantes de Sistema**

```csharp
// Ubicación: UserRoles.Identity/Constants/ModuleRoles.cs

// Roles disponibles
SystemRoles.Admin
SystemRoles.Manager
SystemRoles.Employee
SystemRoles.Viewer

// Módulos
ModuleNames.Compras
ModuleNames.Inventario
ModuleNames.Finanzas
// ... (9 módulos en total)

// Permisos
PermissionNames.View
PermissionNames.Create
PermissionNames.Edit
PermissionNames.Delete
```

### 4. **Configuración en Program.cs**

```csharp
// ✅ Ya configurado
builder.Services.AddScoped<IPermissionService, PermissionService>();
```

---

## 🚀 CÓMO USAR

### **Paso 1: Crear Migraciones**

```bash
cd WebApp
dotnet ef migrations add AddModulePermissions --project ..\UserRoles.Identity
dotnet ef database update
```

### **Paso 2: Proteger Controllers**

**Opción A: Solo roles**
```csharp
[Authorize(Roles = "Admin,Manager")]
public class ComprasController : Controller { }
```

**Opción B: Verificar permisos por módulo**
```csharp
[Authorize]
public class InventarioController : Controller
{
    private readonly IPermissionService _permissionService;

    public InventarioController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!await _permissionService.UserHasModuleAccessAsync(userId, ModuleNames.Inventario))
        {
            return Forbid();
        }

        return View();
    }
}
```

### **Paso 3: Crear Usuario Admin**

```csharp
// En un servicio o controller
var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

// Crear rol
await roleManager.CreateAsync(new IdentityRole(SystemRoles.Admin));

// Crear usuario
var user = new Users 
{ 
    UserName = "admin", 
    Email = "admin@erp.com",
    Fullname = "Administrador",
    EmailConfirmed = true
};

var result = await userManager.CreateAsync(user, "Admin@123456");
if (result.Succeeded)
{
    await userManager.AddToRoleAsync(user, SystemRoles.Admin);
}
```

### **Paso 4: Seed de Permisos Iniciales** (Opcional pero Recomendado)

```csharp
// En Program.cs, después de app.Build()
await SeedDefaultPermissions.Seed(app.Services);
```

### **Paso 5: Actualizar Menú en Layout**

```razor
@inject UserManager<Users> UserManager
@inject IPermissionService PermissionService
@using System.Security.Claims

@{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userModules = userId != null 
        ? await PermissionService.GetUserModulesAsync(userId) 
        : new List<string>();
}

<div class="sb-sidenav-menu">
    <div class="nav">
        @foreach (var module in userModules)
        {
            <a class="nav-link" asp-action="@module" asp-controller="Modules">
                <span>@module</span>
            </a>
        }
    </div>
</div>
```

---

## 📝 Archivos Generados/Modificados

### ✨ Nuevos Archivos

1. **Modelos**
   - `..\UserRoles.Identity\Models\ModulePermission.cs`
   - `..\UserRoles.Identity\Models\RolePermission.cs`

2. **Constantes**
   - `..\UserRoles.Identity\Constants\ModuleRoles.cs`

3. **Servicios**
   - `..\UserRoles.Identity\Services\PermissionService.cs`

4. **Controllers**
   - `WebApp\Controllers\AccessControlController.cs` (ejemplo de implementación)

5. **Páginas de Administración**
   - `WebApp\Pages\Admin\Permissions.cshtml.cs`
   - `WebApp\Pages\Admin\Permissions.cshtml`

6. **Documentación**
   - `GUIA_ACCESO_PAGINAS.md`
   - `IMPLEMENTACION_PERMISOS.md`
   - `README_SISTEMA_ACCESO.md` (este archivo)

### 🔄 Modificados

- **`WebApp\Program.cs`** - Agregado registro de `IPermissionService`
- **`WebApp\Controllers\ModulesController.cs`** - Agregado `[Authorize]` y verificación básica
- **`..\UserRoles.Identity\Data\AppDbContext.cs`** - Agregados DbSets para permisos

---

## 🔒 Patrones de Seguridad

### **1. Autenticación Requerida**
```csharp
[Authorize]
public class MyController : Controller { }
```

### **2. Rol Específico Requerido**
```csharp
[Authorize(Roles = SystemRoles.Admin)]
public IActionResult AdminDashboard() { }
```

### **3. Múltiples Roles**
```csharp
[Authorize(Roles = $"{SystemRoles.Admin},{SystemRoles.Manager}")]
public IActionResult ManagerView() { }
```

### **4. Permiso de Módulo Específico**
```csharp
var canCreate = await _permissionService.UserHasPermissionAsync(
    userId, 
    ModuleNames.Compras, 
    PermissionNames.Create);

if (!canCreate) return Forbid();
```

---

## 🧪 Verificación Rápida

Para verificar que todo está configurado correctamente:

1. ✅ Compilación sin errores → **COMPLETADO**
2. ⏳ Crear migraciones → **PENDIENTE** (ejecutar en paso 1)
3. ⏳ Actualizar base de datos → **PENDIENTE** (ejecutar en paso 1)
4. ⏳ Crear usuario admin → **PENDIENTE** (ejecutar en paso 3)
5. ⏳ Proteger controllers → **PENDIENTE** (aplicar en tus controllers)

---

## 📚 Documentación Adicional

- 📖 `GUIA_ACCESO_PAGINAS.md` - Guía paso a paso completa
- 📖 `IMPLEMENTACION_PERMISOS.md` - Checklist de implementación
- 🔗 `AccessControlController.cs` - Ejemplos de uso

---

## 🎯 Próximos Pasos Recomendados

1. **Ejecutar migraciones de BD** → Crear tablas en SQL Server
2. **Crear usuario administrador** → Primer login
3. **Proteger todos los módulos** → Aplicar `[Authorize]` a cada controller
4. **Asignar permisos a roles** → Configurar acceso por módulo
5. **Crear página de login** → Si no existe (usar Identity)
6. **Crear página de error 403** → Para acceso denegado

---

## ❓ Preguntas Frecuentes

**P: ¿Cómo creo un nuevo usuario?**  
R: Usa `UserManager<Users>.CreateAsync(user, password)`

**P: ¿Cómo asigno un rol a un usuario?**  
R: `await userManager.AddToRoleAsync(user, roleName)`

**P: ¿Cómo modifico permisos de un rol?**  
R: Usa `IPermissionService.AssignPermissionToRoleAsync(roleId, permissionId)`

**P: ¿Qué pasa si un usuario no tiene acceso?**  
R: El método `Forbid()` retorna un error 403 (Access Denied)

**P: ¿Puedo cambiar los nombres de roles?**  
R: Sí, modifica las constantes en `ModuleRoles.cs`

---

## 🆘 Troubleshooting

**Error: "There are pending migrations"**
```bash
dotnet ef database update
```

**Error: "Cannot find type 'PermissionService'"**
→ Asegúrate de que `Program.cs` tiene: `builder.Services.AddScoped<IPermissionService, PermissionService>();`

**Usuario no puede acceder a módulo**
→ Verifica que tenga un rol asignado y que el rol tenga permisos

---

## 📞 Soporte

Para más información sobre ASP.NET Identity:
- https://docs.microsoft.com/aspnet/core/security/authentication/identity
- https://docs.microsoft.com/aspnet/core/security/authorization/

---

**¡Tu sistema de acceso está listo para implementar!** 🎉
