# 🔧 COMANDOS EXACTOS - Copia y Pega

## 1️⃣ CREAR MIGRACIONES

Abre PowerShell en `WebApp` y ejecuta:

```powershell
cd WebApp
dotnet ef migrations add AddModulePermissions --project ..\UserRoles.Identity
```

Resultado esperado:
```
Done. To undo this action, use 'ef migrations remove'
```

## 2️⃣ ACTUALIZAR BASE DE DATOS

```powershell
dotnet ef database update
```

Resultado esperado:
```
Build succeeded.
Done.
```

## 3️⃣ VERIFICAR TABLAS EN SQL SERVER

Abre SQL Server Management Studio y ejecuta:

```sql
-- Verificar que las tablas existen
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('ModulePermissions', 'RolePermissions')

-- Ver estructura
EXEC sp_columns ModulePermissions
EXEC sp_columns RolePermissions
```

## 4️⃣ CREAR ROLES (OPCIONAL - Seed automático)

En un archivo o servicio, ejecuta este código:

```csharp
using Microsoft.AspNetCore.Identity;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Models;

public static class InitializeRoles
{
    public static async Task Create(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var role in SystemRoles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"✓ Rol creado: {role}");
            }
        }
    }
}

// En Program.cs:
// await InitializeRoles.Create(app.Services);
```

## 5️⃣ CREAR USUARIO ADMIN

```csharp
using Microsoft.AspNetCore.Identity;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Models;

public static class CreateAdminUser
{
    public static async Task Create(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();

        var adminUser = new Users
        {
            UserName = "admin",
            Email = "admin@erp.com",
            Fullname = "Administrador del Sistema",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123456");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, SystemRoles.Admin);
            Console.WriteLine("✓ Usuario admin creado exitosamente");
            Console.WriteLine("  Usuario: admin");
            Console.WriteLine("  Contraseña: Admin@123456");
            Console.WriteLine("  Rol: Admin");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"✗ Error: {error.Description}");
            }
        }
    }
}

// En Program.cs:
// await CreateAdminUser.Create(app.Services);
```

## 6️⃣ CREAR PERMISOS (OPCIONAL - Seed automático)

```csharp
using UserRoles.Identity.Constants;
using UserRoles.Identity.Data;
using UserRoles.Identity.Models;

public static class SeedPermissions
{
    public static async Task Create(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        foreach (var module in ModuleNames.AllModules)
        {
            foreach (var permission in new[] { 
                PermissionNames.View, 
                PermissionNames.Create, 
                PermissionNames.Edit, 
                PermissionNames.Delete 
            })
            {
                var exists = context.ModulePermissions
                    .Any(mp => mp.ModuleName == module && mp.PermissionName == permission);

                if (!exists)
                {
                    context.ModulePermissions.Add(new ModulePermission
                    {
                        ModuleName = module,
                        PermissionName = permission,
                        Description = $"{permission} en módulo {module}",
                        IsActive = true
                    });
                }
            }
        }

        await context.SaveChangesAsync();
        Console.WriteLine("✓ Permisos creados");
    }
}

// En Program.cs:
// await SeedPermissions.Create(app.Services);
```

## 7️⃣ ASIGNAR PERMISOS AL ROL ADMIN

```sql
-- SQL: Asignar todos los permisos a Admin
INSERT INTO RolePermissions (RoleId, ModulePermissionId)
SELECT r.Id, mp.Id
FROM AspNetRoles r, ModulePermissions mp
WHERE r.Name = 'Admin'
  AND NOT EXISTS (
    SELECT 1 FROM RolePermissions rp 
    WHERE rp.RoleId = r.Id AND rp.ModulePermissionId = mp.Id
  )
```

## 8️⃣ VERIFICAR USUARIO ADMIN EN BD

```sql
-- Verificar usuario
SELECT * FROM AspNetUsers WHERE UserName = 'admin'

-- Verificar roles del usuario
SELECT r.Name 
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName = 'admin'

-- Verificar permisos del admin
SELECT mp.ModuleName, mp.PermissionName
FROM AspNetRoles r
JOIN RolePermissions rp ON r.Id = rp.RoleId
JOIN ModulePermissions mp ON rp.ModulePermissionId = mp.Id
WHERE r.Name = 'Admin'
ORDER BY mp.ModuleName, mp.PermissionName
```

## 9️⃣ PROBAR LOGIN

1. Inicia la aplicación
2. Intenta acceder sin loguear → Debe redirigir a login
3. Login con: admin / Admin@123456
4. Debe mostrar acceso a todos los módulos

## 🔟 COMPILAR Y EJECUTAR

```powershell
# Compilar
dotnet build

# Ejecutar
dotnet run
```

---

## 📋 CHECKLIST DE COMANDOS

```
☐ cd WebApp
☐ dotnet ef migrations add AddModulePermissions --project ..\UserRoles.Identity
☐ dotnet ef database update
☐ Verificar en SQL Server Management Studio
☐ Crear roles (ejecutar código)
☐ Crear usuario admin (ejecutar código)
☐ Asignar permisos (ejecutar SQL)
☐ Verificar en BD (ejecutar SQL)
☐ dotnet build
☐ dotnet run
☐ Probar login
```

---

## ⚠️ ERRORES COMUNES Y SOLUCIONES

### Error: "No database provider has been configured for this DbContext"

**Solución:** Asegúrate que las migraciones están aplicadas:
```powershell
dotnet ef database update
```

### Error: "There are no migrations"

**Solución:** Crea la migración primero:
```powershell
dotnet ef migrations add AddModulePermissions --project ..\UserRoles.Identity
```

### Error: "A network-related or instance-specific error occurred"

**Solución:** Verifica tu connection string en `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=true;"
}
```

### Error: "UserManager<Users> not found"

**Solución:** Verifica que Program.cs tenga:
```csharp
builder.Services.AddIdentity<Users, IdentityRole>(...)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
```

---

## 🔑 CONTRASEÑA TEMPORAL ADMIN

**Contraseña temporal:** Admin@123456

⚠️ **IMPORTANTE:** Cambia esta contraseña en producción

Para cambiar la contraseña del admin en BD:
```sql
-- NO recomendado, mejor usar UI de Identity
-- Usa la página de cambio de contraseña de tu aplicación
```

---

## 📊 COMANDOS ÚTILES POST-IMPLEMENTACIÓN

```powershell
# Ver migraciones aplicadas
dotnet ef migrations list --project ..\UserRoles.Identity

# Ver estado actual de BD
dotnet ef database info --project ..\UserRoles.Identity

# Script de migraciones (para backup)
dotnet ef migrations script --project ..\UserRoles.Identity --output migrations.sql

# Crear usuario adicional desde consola
# (Usa el código de CreateAdminUser adaptado)
```

---

## 🎯 RESUMEN RÁPIDO

| Paso | Comando | Tiempo |
|------|---------|--------|
| 1 | `dotnet ef migrations add ...` | 30 seg |
| 2 | `dotnet ef database update` | 30 seg |
| 3 | Ejecutar código C# | 1 min |
| 4 | Verificar BD | 1 min |
| 5 | Compilar y ejecutar | 2 min |
| **TOTAL** | | **5 minutos** |

---

## ✅ VERIFICACIÓN FINAL

Después de ejecutar todos los comandos:

```bash
# 1. Compilación exitosa
dotnet build  # ← Debe decir "Build succeeded"

# 2. Aplicación corre
dotnet run    # ← Debe iniciar sin errores

# 3. Puedes loguear
# Login: admin / Admin@123456

# 4. Accedes a módulos
# Deberías ver todos los módulos disponibles
```

---

## 🆘 SOPORTE

Si algo no funciona:

1. Verifica los comandos exactos arriba
2. Lee INICIO_RAPIDO.md
3. Consulta README_SISTEMA_ACCESO.md
4. Revisa los logs en la consola

---

**Última actualización:** 2024  
**Version:** 1.0  
**Estado:** ✅ LISTA PARA COPIAR Y PEGAR
