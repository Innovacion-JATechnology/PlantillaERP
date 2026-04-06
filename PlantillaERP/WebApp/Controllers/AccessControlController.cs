using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Services;

namespace WebApp.Controllers
{
    /// <summary>
    /// Ejemplo de Controller protegido con autenticación y autorización
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    public class AccessControlController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<AccessControlController> _logger;

        public AccessControlController(IPermissionService permissionService, ILogger<AccessControlController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        /// <summary>
        /// Ejemplo: Solo Admin
        /// </summary>
        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            ViewData["Title"] = "Página solo para Administradores";
            return View();
        }

        /// <summary>
        /// Ejemplo: Admin o Manager
        /// </summary>
        [Authorize(Roles = $"{SystemRoles.Admin},{SystemRoles.Manager}")]
        [HttpGet("managers")]
        public IActionResult ManagersView()
        {
            ViewData["Title"] = "Página para Administradores y Managers";
            return View();
        }

        /// <summary>
        /// Ejemplo: Verificar permiso en módulo usando IPermissionService
        /// </summary>
        [HttpGet("compras")]
        public async Task<IActionResult> ComprasPage()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return Unauthorized();
            }

            // Verificar si el usuario tiene acceso al módulo Compras
            var hasAccess = await _permissionService.UserHasModuleAccessAsync(userId, ModuleNames.Compras);
            
            if (!hasAccess)
            {
                return Forbid();
            }

            // Obtener permisos específicos del usuario
            var permissions = await _permissionService.GetUserPermissionsAsync(userId, ModuleNames.Compras);
            
            ViewData["Title"] = "Compras";
            ViewData["UserPermissions"] = permissions;
            
            return View();
        }

        /// <summary>
        /// Verificar si el usuario tiene permiso para crear en Compras
        /// </summary>
        [HttpPost("compras/create")]
        public async Task<IActionResult> CreateCompra()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return Unauthorized();
            }

            // Verificar permiso específico
            var canCreate = await _permissionService.UserHasPermissionAsync(userId, ModuleNames.Compras, PermissionNames.Create);
            
            if (!canCreate)
            {
                return Forbid();
            }

            // Lógica para crear compra
            return Ok("Compra creada exitosamente");
        }

        /// <summary>
        /// Obtener módulos a los que el usuario tiene acceso
        /// </summary>
        [HttpGet("my-modules")]
        public async Task<IActionResult> MyModules()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
            {
                return Unauthorized();
            }

            var modules = await _permissionService.GetUserModulesAsync(userId);
            
            return Json(new { modules });
        }
    }
}
