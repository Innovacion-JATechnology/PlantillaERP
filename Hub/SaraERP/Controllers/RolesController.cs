using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Data;
using UserRoles.Identity.Models;
using WebApp.Attributes;

namespace WebApp.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Users> _userManager;
        private readonly ILogger<RolesController> _logger;
        private readonly AppDbContext _dbContext;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<Users> userManager,
            ILogger<RolesController> logger,
            AppDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;
        }

    

        // POST: Crear nuevo rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("Administracion", "Crear")]
        public async Task<IActionResult> CrearRol([FromBody] CrearRolRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                // Validar que el nombre no esté vacío
                if (string.IsNullOrWhiteSpace(request.NombreRol))
                {
                    return Json(new { success = false, message = "El nombre del rol es requerido" });
                }

                // Validar que el rol no exista
                var rolExistente = await _roleManager.FindByNameAsync(request.NombreRol);
                if (rolExistente != null)
                {
                    return Json(new { success = false, message = "El rol ya existe" });
                }

                var nuevoRol = new IdentityRole
                {
                    Name = request.NombreRol,
                    NormalizedName = request.NombreRol.ToUpper()
                };

                var resultado = await _roleManager.CreateAsync(nuevoRol);

                if (resultado.Succeeded)
                {
                    _logger.LogInformation($"Rol '{request.NombreRol}' creado exitosamente");
                    return Json(new { success = true, message = "Rol creado exitosamente" });
                }
                else
                {
                    var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = $"Error al crear el rol: {errores}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear rol: {ex.Message}");
                return Json(new { success = false, message = "Error al crear el rol" });
            }
        }

        // DELETE: Eliminar rol
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [RequirePermission("Administracion", "Eliminar")]
        public async Task<IActionResult> EliminarRol(string id)
        {
            try
            {
                var rol = await _roleManager.FindByIdAsync(id);
                if (rol == null)
                {
                    return Json(new { success = false, message = "El rol no existe" });
                }

                // Evitar eliminar roles del sistema
                if (rol.Name == "Admin" || rol.Name == "Manager" || rol.Name == "User")
                {
                    return Json(new { success = false, message = "No se pueden eliminar roles del sistema" });
                }

                // Verificar si hay usuarios asignados a este rol
                var usuariosConRol = await _userManager.GetUsersInRoleAsync(rol.Name);
                if (usuariosConRol.Any())
                {
                    var cantidad = usuariosConRol.Count();
                    return Json(new 
                    { 
                        success = false, 
                        message = $"No se puede eliminar el rol '{rol.Name}' porque hay {cantidad} usuario(s) asignado(s) a este rol. Reasigne estos usuarios a otro rol antes de eliminar." 
                    });
                }

                var resultado = await _roleManager.DeleteAsync(rol);

                if (resultado.Succeeded)
                {
                    _logger.LogInformation($"Rol '{rol.Name}' eliminado exitosamente");
                    return Json(new { success = true, message = "Rol eliminado exitosamente" });
                }
                else
                {
                    var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = $"Error al eliminar el rol: {errores}" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar rol: {ex.Message}");
                return Json(new { success = false, message = "Error al eliminar el rol" });
            }
        }

        // GET: Obtener lista de roles para select
        [HttpGet]
        [RequirePermission("Administracion", "Ver")]
        public async Task<IActionResult> ListarRoles()
        {
            try
            {
                var roles = await _roleManager.Roles
                    .Select(r => new
                    {
                        id = r.Id,
                        name = r.Name
                    })
                    .ToListAsync();

                return Json(new { success = true, roles = roles });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener roles: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener roles" });
            }
        }

        // GET: Obtener usuarios de un rol específico
        [HttpGet]
        [RequirePermission("Administracion", "Ver")]
        public async Task<IActionResult> ObtenerUsuariosRol(string roleId)
        {
            try
            {
                var rol = await _roleManager.FindByIdAsync(roleId);
                if (rol == null)
                {
                    return Json(new { success = false, message = "El rol no existe" });
                }

                var usuariosConRol = await _userManager.GetUsersInRoleAsync(rol.Name);

                var usuarios = usuariosConRol.Select(u => new
                {
                    id = u.Id,
                    email = u.Email,
                    nombre = u.UserName
                }).ToList();

                return Json(new 
                { 
                    success = true,
                    roleName = rol.Name,
                    usuarios = usuarios,
                    cantidad = usuarios.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener usuarios del rol: {ex.Message}");
                return Json(new { success = false, message = "Error al obtener usuarios" });
            }
        }

        // POST: Reasignar usuarios de un rol a otro
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("Administracion", "Editar")]
        public async Task<IActionResult> ReasignarRol([FromBody] ReasignarRolRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                // Validar que ambos roles existan
                var rolOrigen = await _roleManager.FindByIdAsync(request.RolOrigenId);
                if (rolOrigen == null)
                {
                    return Json(new { success = false, message = "El rol de origen no existe" });
                }

                var rolDestino = await _roleManager.FindByIdAsync(request.RolDestinoId);
                if (rolDestino == null)
                {
                    return Json(new { success = false, message = "El rol de destino no existe" });
                }

                if (request.RolOrigenId == request.RolDestinoId)
                {
                    return Json(new { success = false, message = "El rol de origen y destino deben ser diferentes" });
                }

                // Obtener usuarios seleccionados
                var usuariosParaReasignar = request.UsuarioIds ?? new List<string>();

                if (!usuariosParaReasignar.Any())
                {
                    return Json(new { success = false, message = "Debe seleccionar al menos un usuario" });
                }

                // Validar que todos los usuarios existan y estén en el rol de origen
                var usuariosVerificados = new List<Users>();
                foreach (var usuarioId in usuariosParaReasignar)
                {
                    var usuario = await _userManager.FindByIdAsync(usuarioId);
                    if (usuario == null)
                    {
                        return Json(new { success = false, message = $"El usuario con ID {usuarioId} no existe" });
                    }

                    var estaEnRolOrigen = await _userManager.IsInRoleAsync(usuario, rolOrigen.Name);
                    if (!estaEnRolOrigen)
                    {
                        return Json(new { success = false, message = $"El usuario {usuario.Email} no está en el rol de origen" });
                    }

                    usuariosVerificados.Add(usuario);
                }

                // Realizar la reasignación
                var erroresReasignacion = new List<string>();

                foreach (var usuario in usuariosVerificados)
                {
                    // Remover del rol origen
                    var resultadoRemover = await _userManager.RemoveFromRoleAsync(usuario, rolOrigen.Name);
                    if (!resultadoRemover.Succeeded)
                    {
                        erroresReasignacion.Add($"Error al remover {usuario.Email} del rol {rolOrigen.Name}");
                        continue;
                    }

                    // Agregar al rol destino
                    var resultadoAgregar = await _userManager.AddToRoleAsync(usuario, rolDestino.Name);
                    if (!resultadoAgregar.Succeeded)
                    {
                        // Intentar restaurar el rol anterior si falla
                        await _userManager.AddToRoleAsync(usuario, rolOrigen.Name);
                        erroresReasignacion.Add($"Error al agregar {usuario.Email} al rol {rolDestino.Name}");
                    }
                }

                if (erroresReasignacion.Any())
                {
                    var erroresStr = string.Join("; ", erroresReasignacion);
                    _logger.LogWarning($"Errores durante reasignación de roles: {erroresStr}");
                    return Json(new 
                    { 
                        success = false, 
                        message = $"Ocurrieron errores: {erroresStr}",
                        usuariosReasignados = usuariosVerificados.Count - erroresReasignacion.Count
                    });
                }

                _logger.LogInformation($"Se reasignaron {usuariosVerificados.Count} usuarios de '{rolOrigen.Name}' a '{rolDestino.Name}'");

                return Json(new 
                { 
                    success = true,
                    message = $"Se reasignaron exitosamente {usuariosVerificados.Count} usuario(s) al rol '{rolDestino.Name}'",
                    usuariosReasignados = usuariosVerificados.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al reasignar roles: {ex.Message}");
                return Json(new { success = false, message = "Error al reasignar roles" });
            }
        }

        // DTO para crear rol
        public class CrearRolRequest
        {
            [Required]
            [StringLength(256)]
            public string NombreRol { get; set; }
        }

        // DTO para reasignar roles
        public class ReasignarRolRequest
        {
            [Required]
            public string RolOrigenId { get; set; }

            [Required]
            public string RolDestinoId { get; set; }

            public List<string> UsuarioIds { get; set; } = new List<string>();
        }
    }
}
