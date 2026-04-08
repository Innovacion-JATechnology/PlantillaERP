using Microsoft.AspNetCore.Identity;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Data;
using UserRoles.Identity.Models;

namespace WebApp
{
    public static class SeedInitialData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                // 1. Crear roles
                foreach (var role in SystemRoles.AllRoles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        Console.WriteLine($"✅ Rol '{role}' creado");
                    }
                }

                // 2. Crear permisos por módulo
                var modulesCreated = 0;
                foreach (var module in ModuleNames.AllModules)
                {
                    foreach (var permission in new[] { 
                        PermissionNames.View, 
                        PermissionNames.Create, 
                        PermissionNames.Edit, 
                        PermissionNames.Delete 
                    })
                    {
                        var exists = context.Set<ModulePermission>()
                            .Any(mp => mp.ModuleName == module && mp.PermissionName == permission);
                        
                        if (!exists)
                        {
                            context.Set<ModulePermission>().Add(new ModulePermission
                            {
                                ModuleName = module,
                                PermissionName = permission,
                                Description = $"{permission} en {module}",
                                IsActive = true
                            });
                            modulesCreated++;
                        }
                    }
                }

                if (modulesCreated > 0)
                {
                    await context.SaveChangesAsync();
                    Console.WriteLine($"✅ {modulesCreated} permisos creados");
                }

                // 3. Crear usuario administrador
                var adminEmail = "admin@erp.com";
                var adminUserByEmail = await userManager.FindByEmailAsync(adminEmail);
                var adminUserByName = await userManager.FindByNameAsync("admin");

                if (adminUserByEmail == null && adminUserByName == null)
                {
                    var adminUser = new Users
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Fullname = "Administrador del Sistema"
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123456");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, SystemRoles.Admin);
                        Console.WriteLine("✅ Usuario admin creado exitosamente");
                        Console.WriteLine("   ➜ Usuario (Username): admin");
                        Console.WriteLine("   ➜ Email: admin@erp.com");
                        Console.WriteLine("   ➜ Contraseña: Admin@123456");
                        Console.WriteLine("   ➜ Rol: Admin");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                        Console.WriteLine($"❌ Error al crear admin: {errors}");
                    }
                }
                else
                {
                    Console.WriteLine("⚠️ Usuario admin ya existe en el sistema");
                    if (adminUserByName != null)
                        Console.WriteLine($"   ➜ Encontrado por nombre: {adminUserByName.UserName}");
                    if (adminUserByEmail != null)
                        Console.WriteLine($"   ➜ Encontrado por email: {adminUserByEmail.Email}");
                }

                // 4. Asignar todos los permisos al rol Admin
                var adminRole = await roleManager.FindByNameAsync(SystemRoles.Admin);
                if (adminRole != null)
                {
                    var permissions = context.Set<ModulePermission>().ToList();
                    var adminPermissions = context.Set<RolePermission>()
                        .Where(rp => rp.RoleId == adminRole.Id)
                        .ToList();

                    var permissionsToAdd = permissions.Where(p => !adminPermissions.Any(ap => ap.ModulePermissionId == p.Id)).ToList();

                    if (permissionsToAdd.Count > 0)
                    {
                        foreach (var perm in permissionsToAdd)
                        {
                            context.Set<RolePermission>().Add(new RolePermission
                            {
                                RoleId = adminRole.Id,
                                ModulePermissionId = perm.Id
                            });
                        }
                        await context.SaveChangesAsync();
                        Console.WriteLine($"✅ {permissionsToAdd.Count} permisos asignados al rol Admin");
                    }
                }

                Console.WriteLine("\n✅ Datos iniciales creados correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear datos iniciales: {ex.Message}");
                throw;
            }
        }
    }
}
