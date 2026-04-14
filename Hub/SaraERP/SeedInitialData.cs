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

                // 2. Crear permisos por submódulo
                var modulesCreated = 0;
                var submodules = new[]
                {
                    // Compras
                    "Compras.ComprasInternacionales", "Compras.Proveedores", "Compras.Solicitudes",
                    "Compras.OrdenesCompra", "Compras.ReportesCompras", "Compras.Contratos",
                    // Inventario
                    "Inventario.Catalogo", "Inventario.Stock", "Inventario.Almacenes",
                    "Inventario.Categorias", "Inventario.Movimientos", "Inventario.AuditoriaInventario",
                    "Inventario.AjustesInventario", "Inventario.ReportesInventario",
                    // Finanzas
                    "Finanzas.Contabilidad", "Finanzas.Facturas", "Finanzas.ReportesFinancieros",
                    "Finanzas.Presupuestos", "Finanzas.CuentasPorPagar", "Finanzas.CuentasPorCobrar",
                    "Finanzas.Conciliaciones", "Finanzas.FlujoCaja",
                    // Mantenimiento
                    "Mantenimiento.Equipos", "Mantenimiento.OrdenesMantenimiento", "Mantenimiento.ReportesMantenimiento",
                    "Mantenimiento.MantenimientoPreventivo", "Mantenimiento.MantenimientoCorrectivo",
                    "Mantenimiento.ChecklistMantenimiento", "Mantenimiento.HistoricoMantenimiento",
                    // Producción
                    "Produccion.OrdenesProduccion", "Produccion.Recursos", "Produccion.ControlCalidad",
                    "Produccion.Procesos", "Produccion.MaterialesProduccion", "Produccion.ProductosFinales",
                    "Produccion.ReportesProduccion",
                    // RRHH
                    "RRHH.Empleados", "RRHH.Nomina", "RRHH.Capacitacion", "RRHH.Evaluaciones",
                    "RRHH.Beneficios", "RRHH.Asistencia", "RRHH.PermisosVacaciones", "RRHH.ReportesRRHH",
                    // Proyectos
                    "Proyectos.ListaProyectos", "Proyectos.Tareas", "Proyectos.RecursosProyecto",
                    "Proyectos.DiagramaGantt", "Proyectos.PresupuestoProyectos", "Proyectos.DocumentosProyectos",
                    "Proyectos.ReportesProyectos"
                };

                foreach (var submodule in submodules)
                {
                    foreach (var permission in new[] { 
                        PermissionNames.View, 
                        PermissionNames.Create, 
                        PermissionNames.Edit, 
                        PermissionNames.Delete 
                    })
                    {
                        var exists = context.Set<ModulePermission>()
                            .Any(mp => mp.ModuleName == submodule && mp.PermissionName == permission);

                        if (!exists)
                        {
                            context.Set<ModulePermission>().Add(new ModulePermission
                            {
                                ModuleName = submodule,
                                PermissionName = permission,
                                Description = $"{permission} en {submodule}",
                                IsActive = true
                            });
                            modulesCreated++;
                        }
                    }
                }

                if (modulesCreated > 0)
                {
                    await context.SaveChangesAsync();
                    Console.WriteLine($"✅ {modulesCreated} permisos de submódulos creados");
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
