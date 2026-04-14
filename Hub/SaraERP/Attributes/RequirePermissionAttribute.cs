using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserRoles.Identity.Data;

namespace WebApp.Attributes
{
    /// <summary>
    /// Atributo personalizado para requerir permisos específicos
    /// Uso: [RequirePermission("Usuarios", "Crear")]
    /// </summary>
    public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _moduleName;
        private readonly string _permissionName;

        public RequirePermissionAttribute(string moduleName, string permissionName)
        {
            _moduleName = moduleName;
            _permissionName = permissionName;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Si no está autenticado, denegar acceso
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Si el usuario es Admin, permitir acceso automáticamente
            if (context.HttpContext.User.IsInRole("Admin"))
            {
                return;
            }

            // Obtener el DbContext
            var dbContext = context.HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            if (dbContext == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Obtener IDs de roles del usuario desde UserManager
            var userClaimsPrincipal = context.HttpContext.User;
            var userManager = context.HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Identity.UserManager<UserRoles.Identity.Models.Users>)) as Microsoft.AspNetCore.Identity.UserManager<UserRoles.Identity.Models.Users>;

            if (userManager == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userId = userClaimsPrincipal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userRoleNames = await userManager.GetRolesAsync(user);

            // Verificar si el usuario tiene el permiso requerido
            var tienePermiso = await dbContext.RolePermissions
                .Include(rp => rp.ModulePermission)
                .Join(
                    dbContext.Roles.Where(r => userRoleNames.Contains(r.Name)),
                    rp => rp.RoleId,
                    role => role.Id,
                    (rp, role) => rp
                )
                .AnyAsync(rp =>
                    rp.ModulePermission.ModuleName == _moduleName &&
                    rp.ModulePermission.PermissionName == _permissionName &&
                    rp.ModulePermission.IsActive
                );

            if (!tienePermiso)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
