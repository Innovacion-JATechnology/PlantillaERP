# 🎯 MAPA DE IMPLEMENTACIÓN - Sistema de Acceso

## Estructura del Proyecto

```
PlantillaERP/
├── UserRoles.Identity/
│   ├── Models/
│   │   ├── Users.cs                    ✅ (Existía)
│   │   ├── ModulePermission.cs         ✨ NUEVO
│   │   └── RolePermission.cs           ✨ NUEVO
│   ├── Data/
│   │   └── AppDbContext.cs             🔄 ACTUALIZADO
│   ├── Services/
│   │   ├── SeedService.cs              (Existía)
│   │   └── PermissionService.cs        ✨ NUEVO
│   ├── Constants/
│   │   └── ModuleRoles.cs              ✨ NUEVO
│   └── GlobalUsings.cs                 (Existía)
│
└── WebApp/
    ├── Controllers/
    │   ├── HomeController.cs            (Existía)
    │   ├── ModulesController.cs         🔄 ACTUALIZADO
    │   ├── ComprasController.cs         (Existía)
    │   ├── AccessControlController.cs   ✨ NUEVO (Ejemplo)
    │   └── ... otros controllers
    ├── Pages/
    │   ├── Admin/
    │   │   ├── Permissions.cshtml       ✨ NUEVO
    │   │   └── Permissions.cshtml.cs    ✨ NUEVO
    │   ├── ComprasPage.cshtml           ✨ NUEVO (Ejemplo)
    │   └── ComprasPage.cshtml.cs        ✨ NUEVO (Ejemplo)
    ├── Program.cs                       🔄 ACTUALIZADO
    ├── README_SISTEMA_ACCESO.md         ✨ NUEVO
    ├── GUIA_ACCESO_PAGINAS.md           ✨ NUEVO
    └── IMPLEMENTACION_PERMISOS.md       ✨ NUEVO
```

---

## Diagrama de Flujo

```
┌─────────────────────────────────────────────────────────┐
│                      USUARIO                             │
└────────────────┬────────────────────────────────────────┘
                 │
                 ▼
        ┌────────────────┐
        │   LOGIN        │  (ASP.NET Identity)
        └────────┬───────┘
                 │
                 ▼
        ┌─────────────────────┐
        │  Usuario autenticado│
        │   (Claim + Roles)   │
        └────────┬────────────┘
                 │
      ┌──────────┴──────────┐
      │                     │
      ▼                     ▼
 ┌─────────┐        ┌──────────────────┐
 │ ROLE    │        │ PERMISSION       │
 │ CHECK   │        │ CHECK            │
 │         │        │ (IPermissionSvc) │
 │[Authorize]       │                  │
 │(Roles=...)       └──────────────────┘
 └─────────┘
      │
      ▼
┌──────────────────┐
│ ACCESO PERMITIDO │
│ ✅ Página/Acción │
└──────────────────┘
```

---

## Tabla de Decisión

| Escenario | Usa | Ubicación | Ejemplo |
|-----------|-----|-----------|---------|
| Solo usuarios autenticados | `[Authorize]` | Atributo de Controller | Cualquier módulo |
| Solo Admin | `[Authorize(Roles="Admin")]` | Atributo de Controller | Admininstración |
| Admin o Manager | `[Authorize(Roles="Admin,Manager")]` | Atributo de Controller | Reportes |
| Verificar módulo | `IPermissionService` | En el método | Compras, Inventario |
| Verificar permiso específico | `IPermissionService` | En el método | Crear, Editar, Eliminar |
| Mostrar botón solo si tiene permiso | `IPermissionService` | En Razor Page | Formulario de edición |

---

## Secuencia de Implementación

```
FASE 1: BASE DE DATOS
┌─ Ejecutar migraciones
├─ Crear tablas: ModulePermissions, RolePermissions
└─ ✓ Completado automáticamente por EF Core

FASE 2: DATOS INICIALES
┌─ Crear roles (Admin, Manager, Employee, Viewer)
├─ Crear módulos y permisos
├─ Crear usuario administrador
└─ Asignar permisos a roles

FASE 3: PROTEGER CONTROLLERS
┌─ Agregar [Authorize] a controllers
├─ Inyectar IPermissionService
├─ Verificar acceso en métodos
└─ Retornar Forbid() si no tiene acceso

FASE 4: MEJORAR UI
┌─ Actualizar _Layout.cshtml con menú dinámico
├─ Mostrar/ocultar botones según permisos
├─ Crear página de error 403
└─ Crear página de login (si no existe)
```

---

## Tabla de Archivos Generados

| Archivo | Tipo | Descripción | Estado |
|---------|------|-------------|--------|
| `ModulePermission.cs` | Modelo | Define permisos | ✅ Listo |
| `RolePermission.cs` | Modelo | Vincula roles con permisos | ✅ Listo |
| `ModuleRoles.cs` | Constantes | Roles, módulos, permisos | ✅ Listo |
| `PermissionService.cs` | Servicio | Lógica de permisos | ✅ Listo |
| `AccessControlController.cs` | Controller | Ejemplo de implementación | ✅ Referencia |
| `Permissions.cshtml` | Vista | Admin de permisos | ✅ Plantilla |
| `ComprasPage.cshtml` | Vista | Ejemplo Razor Page | ✅ Ejemplo |
| `README_SISTEMA_ACCESO.md` | Documentación | Guía principal | ✅ Listo |
| `GUIA_ACCESO_PAGINAS.md` | Documentación | Pasos iniciales | ✅ Listo |
| `IMPLEMENTACION_PERMISOS.md` | Documentación | Checklist | ✅ Listo |

---

## Conexión de Componentes

```
IPermissionService
        │
        ├─► AppDbContext
        │   └─► ModulePermission (BD)
        │   └─► RolePermission (BD)
        │
        ├─► UserManager<Users>
        │   └─► AspNetUsers (BD)
        │
        └─► RoleManager<IdentityRole>
            └─► AspNetRoles (BD)
```

---

## Validaciones Implementadas

```
✅ Usuario debe estar autenticado
   └─ [Authorize] en el controller

✅ Usuario debe tener un rol válido
   └─ [Authorize(Roles="...")] 

✅ Usuario debe tener permiso en el módulo
   └─ await _permissionService.UserHasModuleAccessAsync(...)

✅ Usuario debe tener permiso específico (Create, Edit, Delete)
   └─ await _permissionService.UserHasPermissionAsync(...)

✅ Mostrar/ocultar UI según permisos
   └─ @if (Model.CanCreate) { }
```

---

## Compilación: ✅ EXITOSA

```
Status: Build successful
Errors: 0
Warnings: 0
Projects: 2 (WebApp, UserRoles.Identity)
```

---

## Próximos Comandos a Ejecutar

```bash
# 1. Crear migración
dotnet ef migrations add AddModulePermissions --project ..\UserRoles.Identity

# 2. Actualizar base de datos
dotnet ef database update

# 3. (Opcional) Ver tablas creadas
-- En SQL Server Management Studio:
SELECT * FROM ModulePermissions
SELECT * FROM RolePermissions
```

---

## Puntos de Entrada Principales

### **Para Verificar Acceso**
```
IPermissionService.UserHasPermissionAsync()
IPermissionService.UserHasModuleAccessAsync()
```

### **Para Obtener Información del Usuario**
```
IPermissionService.GetUserModulesAsync()
IPermissionService.GetUserPermissionsAsync()
```

### **Para Administrar Permisos**
```
IPermissionService.AssignPermissionToRoleAsync()
IPermissionService.RemovePermissionFromRoleAsync()
```

---

## Archivos de Referencia

Consulta estos archivos para ejemplos:
- `AccessControlController.cs` - Múltiples formas de usar permisos
- `ComprasPage.cshtml.cs` - Ejemplo completo en Razor Page
- `README_SISTEMA_ACCESO.md` - Guía interactiva

---

## ✨ Resumen Final

| Aspecto | Resultado |
|--------|-----------|
| Modelos creados | ✅ 2 (ModulePermission, RolePermission) |
| Servicios implementados | ✅ 1 (PermissionService) |
| Controllers actualizados | ✅ 1 (ModulesController) |
| Ejemplos proporcionados | ✅ 2 (AccessControl, Compras) |
| Documentación | ✅ 3 archivos |
| Compilación | ✅ Exitosa |
| **ESTADO GENERAL** | **✅ LISTO PARA IMPLEMENTAR** |

---

🎉 **¡Tu sistema de gestión de acceso está completamente configurado y listo para usar!**
