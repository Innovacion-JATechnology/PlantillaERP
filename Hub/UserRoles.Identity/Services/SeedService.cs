using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UserRoles.Identity.Data;
using UserRoles.Identity.Models;

namespace UserRoles.Identity.Services
{
    public class SeedService
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider) 
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

            try
            {
                // Ensure the database is ready
                logger.LogInformation("Asegurando la creación de la base de datos");
                await context.Database.EnsureCreatedAsync();

                // add roles
                logger.LogInformation("Seeding roles.");
                await AddRoleAsync(roleManager, "Admin");
                await AddRoleAsync(roleManager, "User");
                await AddRoleAsync(roleManager, "Manager");
                await AddRoleAsync(roleManager, "Employee");
                await AddRoleAsync(roleManager, "Supervisor");

                // Add admin user
                logger.LogInformation("Sembrando usuario admin");
                var adminEmail = "innovacion@jatechnology.net";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new Users
                    {
                        Fullname = "Gabriel Mendiola Anda",
                        UserName = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var result = await userManager.CreateAsync(adminUser,"Admin@123");
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Asignando rol de administrador al admin");
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogError("Fallo al crear usuario Admin: {Errors}",string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error mientras sembrando la base de datos.");
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Fallo al crear el rol '{roleName}':{string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
