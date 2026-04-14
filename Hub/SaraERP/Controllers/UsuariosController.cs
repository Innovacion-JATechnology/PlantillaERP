using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UserRoles.Identity.Models;
using WebApp.Attributes;

namespace WebApp.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UsuariosController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // POST: Crear nuevo usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermission("Usuarios", "Crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                // Validar que el email sea único
                var usuarioExistente = await _userManager.FindByEmailAsync(request.Email);
                if (usuarioExistente != null)
                {
                    return Json(new { success = false, message = "El correo electrónico ya está registrado" });
                }

                var nuevoUsuario = new Users
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Fullname = request.Fullname,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(nuevoUsuario, request.Password);

                if (!result.Succeeded)
                {
                    var errores = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Json(new { success = false, message = $"Error al crear usuario: {errores}" });
                }

                // Asignar rol si se proporciona
                if (!string.IsNullOrEmpty(request.Rol))
                {
                    await _userManager.AddToRoleAsync(nuevoUsuario, request.Rol);
                }

                _logger.LogInformation($"Nuevo usuario creado: {request.UserName} ({request.Email})");

                return Json(new { success = true, message = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return Json(new { success = false, message = "Error al crear el usuario" });
            }
        }

        // DELETE: Eliminar usuario
        [HttpDelete]
        [RequirePermission("Usuarios", "Eliminar")]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(id);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }

                var result = await _userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    return Json(new { success = false, message = "Error al eliminar el usuario" });
                }

                _logger.LogInformation($"Usuario eliminado: {usuario.UserName}");
                return Json(new { success = true, message = "Usuario eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario");
                return Json(new { success = false, message = "Error al eliminar el usuario" });
            }
        }

        // POST: Cambiar rol del usuario
        [HttpPost]
        [RequirePermission("Usuarios", "Editar")]
        public async Task<IActionResult> CambiarRol(string userId, string nuevoRol)
        {
            try
            {
                var usuario = await _userManager.FindByIdAsync(userId);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }

                var rolesActuales = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);

                if (!string.IsNullOrEmpty(nuevoRol))
                {
                    await _userManager.AddToRoleAsync(usuario, nuevoRol);
                }

                _logger.LogInformation($"Rol actualizado para usuario: {usuario.UserName}");
                return Json(new { success = true, message = "Rol actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar rol");
                return Json(new { success = false, message = "Error al cambiar el rol" });
            }
        }
    }
}

