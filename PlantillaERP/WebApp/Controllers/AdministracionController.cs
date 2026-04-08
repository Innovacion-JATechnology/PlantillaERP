using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoles.Identity.Models;
using WebApp.Attributes;

namespace WebApp.Controllers
{
    [Authorize]
    public class AdministracionController : Controller
    {
        private readonly ILogger<AdministracionController> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministracionController(
            ILogger<AdministracionController> logger,
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
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
