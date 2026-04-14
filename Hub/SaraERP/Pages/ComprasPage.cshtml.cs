using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Services;

namespace WebApp.Pages
{
    /// <summary>
    /// Ejemplo de Razor Page protegida con verificación de permisos
    /// Ubicación: Pages/ComprasPage.cshtml.cs
    /// </summary>
    [Authorize]
    public class ComprasPageModel : PageModel
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<ComprasPageModel> _logger;

        public List<string> UserPermissions { get; set; } = new();
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public ComprasPageModel(IPermissionService permissionService, ILogger<ComprasPageModel> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Verificar acceso al módulo Compras
            var hasAccess = await _permissionService.UserHasModuleAccessAsync(userId, ModuleNames.Compras);
            if (!hasAccess)
            {
                return StatusCode(403, "No tienes acceso a este módulo");
            }

            // Obtener permisos del usuario en Compras
            UserPermissions = await _permissionService.GetUserPermissionsAsync(userId, ModuleNames.Compras);

            // Establecer permisos específicos para usar en la vista
            CanCreate = UserPermissions.Contains(PermissionNames.Create);
            CanEdit = UserPermissions.Contains(PermissionNames.Edit);
            CanDelete = UserPermissions.Contains(PermissionNames.Delete);

            _logger.LogInformation($"Usuario {userId} accedió a Compras. Permisos: {string.Join(", ", UserPermissions)}");

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar permiso de crear
            var canCreate = await _permissionService.UserHasPermissionAsync(
                userId,
                ModuleNames.Compras,
                PermissionNames.Create);

            if (!canCreate)
            {
                return Unauthorized();
            }

            // Lógica de crear compra aquí
            _logger.LogInformation($"Usuario {userId} creó una compra");

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar permiso de editar
            var canEdit = await _permissionService.UserHasPermissionAsync(
                userId,
                ModuleNames.Compras,
                PermissionNames.Edit);

            if (!canEdit)
            {
                return Unauthorized();
            }

            // Lógica de editar compra aquí
            _logger.LogInformation($"Usuario {userId} editó la compra {id}");

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar permiso de eliminar
            var canDelete = await _permissionService.UserHasPermissionAsync(
                userId,
                ModuleNames.Compras,
                PermissionNames.Delete);

            if (!canDelete)
            {
                return Unauthorized();
            }

            // Lógica de eliminar compra aquí
            _logger.LogInformation($"Usuario {userId} eliminó la compra {id}");

            return RedirectToPage();
        }
    }
}
