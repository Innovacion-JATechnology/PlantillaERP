using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoles.Identity.Data;
using UserRoles.Identity.Models;
using UserRoles.Identity.Services;
using WebApp.Attributes;

namespace WebApp.Controllers
{
    [Authorize]
    public class AdministracionController : Controller
    {
        private readonly ILogger<AdministracionController> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPermissionService _permissionService;
        private readonly AppDbContext _context;

        public AdministracionController(
            ILogger<AdministracionController> logger,
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager,
            IPermissionService permissionService,
            AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionService = permissionService;
            _context = context;
        }

        [RequirePermission("Usuarios", "Ver")]
        public async Task<IActionResult> Usuarios()
        {
            ViewData["Title"] = "Usuarios";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Usuarios", null)
            };

            // Obtener usuarios con sus roles
            var usuarios = await _userManager.Users.ToListAsync();
            var usuariosConRoles = new List<UsuarioViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                usuariosConRoles.Add(new UsuarioViewModel
                {
                    Id = usuario.Id,
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    Fullname = usuario.Fullname,
                    Roles = roles.ToList(),
                    EmailConfirmed = usuario.EmailConfirmed,
                    LockoutEnd = usuario.LockoutEnd
                });
            }

            // Pasar roles al ViewBag
            var roles_list = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles_list;

            return View("~/Views/Modules/Usuarios.cshtml", usuariosConRoles);
        }

        [RequirePermission("Administracion", "Ver")]
        public async Task<IActionResult> Roles()
        {
            ViewData["Title"] = "Roles";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Roles", null)
            };

            var roles = await _roleManager.Roles.ToListAsync();
            return View("~/Views/Modules/Roles.cshtml", roles);
        }

        [RequirePermission("Administracion", "Ver")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PermisosSubmodulos(string roleId)
        {
            ViewData["Title"] = "Permisos de Submódulos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Roles", Url.Action("Roles", "Administracion")),
                ("Permisos de Submódulos", null)
            };

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles;

            if (string.IsNullOrEmpty(roleId) && roles.Any())
            {
                roleId = roles.First().Id;
            }

            ViewBag.SelectedRoleId = roleId;

            // Load role permissions for submodules - OBTENER DINÁMICAMENTE
            var roleSubmodulePermissions = new Dictionary<string, List<string>>();
            if (!string.IsNullOrEmpty(roleId))
            {
                // Obtener todos los módulos dinámicamente desde la BD
                var allModules = await _permissionService.GetAllModulesAsync();

                // Para cada módulo, obtener sus submódulos
                foreach (var module in allModules)
                {
                    var modulePermissions = await _permissionService.GetModulePermissionsAsync(module);

                    // Para cada submódulo único en este módulo
                    var submodules = modulePermissions
                        .Select(mp => mp.ModuleName)
                        .Distinct()
                        .ToList();

                    foreach (var submodule in submodules)
                    {
                        roleSubmodulePermissions[submodule] = new List<string>();

                        // Obtener todos los permisos para este submódulo
                        var submodulePermissions = modulePermissions
                            .Where(mp => mp.ModuleName == submodule && mp.IsActive)
                            .ToList();

                        foreach (var permission in submodulePermissions)
                        {
                            // Verificar si el rol tiene este permiso
                            var hasPermission = await _context.RolePermissions
                                .AnyAsync(rp => rp.RoleId == roleId && rp.ModulePermissionId == permission.Id);

                            if (hasPermission)
                            {
                                roleSubmodulePermissions[submodule].Add(permission.PermissionName);
                            }
                        }
                    }
                }
            }

            ViewBag.RoleSubmodulePermissions = roleSubmodulePermissions;
            return View("~/Views/Modules/PermisosSubmodulos.cshtml");
        }

        [HttpPost]
        [RequirePermission("Administracion", "Editar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActualizarPermisoSubmodulo(string roleId, string submoduleName, string actionName, string returnUrl = null)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return BadRequest("Rol no encontrado");
                }

                // Get the module permissions for this submodule + action
                var modulePermissions = await _permissionService.GetModulePermissionsAsync(submoduleName);
                var permission = modulePermissions?.FirstOrDefault(mp => 
                    mp.ModuleName == submoduleName && 
                    mp.PermissionName == actionName && 
                    mp.IsActive);

                if (permission == null)
                {
                    return BadRequest($"Permiso no encontrado para {submoduleName} - {actionName}");
                }

                // Check if role already has this permission
                var hasPermission = await _context.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.ModulePermissionId == permission.Id);

                if (hasPermission)
                {
                    await _permissionService.RemovePermissionFromRoleAsync(roleId, permission.Id);
                }
                else
                {
                    await _permissionService.AssignPermissionToRoleAsync(roleId, permission.Id);
                }

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("PermisosSubmodulos", new { roleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar permiso");
                return BadRequest($"Error al actualizar el permiso: {ex.Message}");
            }
        }

        public IActionResult RolesPermisos()
        {
            ViewData["Title"] = "Roles y Permisos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Roles y Permisos", null)
            };
            return View("~/Views/Modules/RolesPermisos.cshtml");
        }

        public IActionResult Configuracion()
        {
            ViewData["Title"] = "Configuración";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Configuración", null)
            };
            return View("~/Views/Modules/Configuracion.cshtml");
        }

        public IActionResult Auditoria()
        {
            ViewData["Title"] = "Auditoría";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Auditoría", null)
            };
            return View("~/Views/Modules/Auditoria.cshtml");
        }

        public IActionResult Respaldos()
        {
            ViewData["Title"] = "Respaldos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Respaldos", null)
            };
            return View("~/Views/Modules/Respaldos.cshtml");
        }

        public IActionResult ParametrosGenerales()
        {
            ViewData["Title"] = "Parámetros Generales";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Parámetros Generales", null)
            };
            return View("~/Views/Modules/ParametrosGenerales.cshtml");
        }

        public IActionResult Logs()
        {
            ViewData["Title"] = "Logs";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Logs", null)
            };
            return View("~/Views/Modules/Logs.cshtml");
        }

        public IActionResult Integraciones()
        {
            ViewData["Title"] = "Integraciones";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules")),
                ("Integraciones", null)
            };
            return View("~/Views/Modules/Integraciones.cshtml");
        }
    }

    // ViewModels
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public List<string> Roles { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

        public string Estado => LockoutEnd?.UtcDateTime > DateTime.UtcNow ? "Bloqueado" : "Activo";
    }

    public class CrearUsuarioRequest
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre de usuario es requerido")]
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string UserName { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El correo electrónico es requerido")]
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El nombre completo es requerido")]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Fullname { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "La contraseña es requerida")]
        [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Password { get; set; }

        public string Rol { get; set; }
    }
}
