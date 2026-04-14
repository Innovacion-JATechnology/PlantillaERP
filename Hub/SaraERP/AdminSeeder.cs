 



using Microsoft.AspNetCore.Identity;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Models;
namespace WebApp
{ 

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Users>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Crear rol Admin
        if (!await roleManager.RoleExistsAsync(SystemRoles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole(SystemRoles.Admin));
        }

        // Crear usuario admin
        var admin = await userManager.FindByEmailAsync("admin@erp.com");
        if (admin == null)
        {
            admin = new Users
            {
                UserName = "admin",
                Email = "admin@erp.com",
                Fullname = "Administrador",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin@123456");
            await userManager.AddToRoleAsync(admin, SystemRoles.Admin);
        }
    }
}
     
}