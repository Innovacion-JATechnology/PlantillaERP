# Guía: Integración de UserRoles.Identity para Gestionar Acceso a Páginas

## ✅ Estado Actual
- ASP.NET Identity está configurado en `Program.cs`
- Se han creado modelos para permisos y roles
- `IPermissionService` está disponible para gestionar permisos

## 🔧 Pasos para Implementar

### 1. **Actualizar la Base de Datos**

```bash
# En la carpeta raíz del proyecto
dotnet ef migrations add AddModulePermissions --project UserRoles.Identity
dotnet ef database update
```

### 2. **Proteger Controllers con `[Authorize]`**

```csharp
[Authorize]  // Solo usuarios autenticados
public class ComprasController : Controller
{
    [Authorize(Roles = "Admin,Manager")]  // Solo estos roles
    public IActionResult Index()
    {
        return View();
    }
}
```

### 3. **Usar PermissionService en Controllers**

```csharp
public class ModulesController : Controller
{
    private readonly IPermissionService _permissionService;

    public ModulesController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<IActionResult> Compras()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Verificar acceso al módulo
        if (!await _permissionService.UserHasModuleAccessAsync(userId, "Compras"))
        {
            return Forbid();
        }

        return View();
    }
}
```

### 4. **Crear Página de Administración de Roles**

Crear un controller para asignar permisos:

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IPermissionService _permissionService;
    private readonly RoleManager<IdentityRole> _roleManager;

    // Asignar permisos a roles
    public async Task AssignPermissionToRole(string roleId, int permissionId)
    {
        await _permissionService.AssignPermissionToRoleAsync(roleId, permissionId);
    }
}
```

### 5. **Mostrar Menú Dinámico en Layout**

Modificar `_Layout.cshtml` para mostrar solo módulos disponibles:

```razor
@inject IPermissionService PermissionService
@{
    var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userModules = userId != null 
        ? await PermissionService.GetUserModulesAsync(userId) 
        : new List<string>();
}

@foreach (var module in userModules)
{
    <a class="nav-link" asp-action="@module" asp-controller="Modules">
        <span>@module</span>
    </a>
}
```

## 📋 Valores por Defecto

### Roles
- `Admin` - Acceso completo
- `Manager` - Acceso parcial con supervisión
- `Employee` - Acceso limitado
- `Viewer` - Solo lectura

### Módulos
- Compras
- Inventario
- Finanzas
- Mantenimiento
- Produccion
- RRHH
- Proyectos
- Reporteador
- Administracion

### Permisos
- `Ver` - Lectura
- `Crear` - Crear registros
- `Editar` - Modificar registros
- `Eliminar` - Eliminar registros

## 🔐 Atributos de Autorización

### Solo Autenticados
```csharp
[Authorize]
public IActionResult Protegido() { }
```

### Rol Específico
```csharp
[Authorize(Roles = "Admin")]
public IActionResult AdminPanel() { }
```

### Múltiples Roles
```csharp
[Authorize(Roles = "Admin,Manager")]
public IActionResult Reportes() { }
```

### Política Personalizada (Opcional)
```csharp
// En Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ComprasAccess", policy =>
        policy.Requirements.Add(new ComprasPermissionRequirement()));
});
```

## 📝 Próximos Pasos

1. Ejecutar migraciones para crear tablas en BD
2. Crear Razor Page para administración de permisos
3. Seed de roles y permisos iniciales
4. Proteger cada controller según necesidad
5. Crear vista de login si no existe
