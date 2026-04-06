using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Data;
using UserRoles.Identity.Models;

namespace UserRoles.Identity.Services
{
    /// <summary>
    /// Servicio para gestionar permisos de módulos y usuarios
    /// </summary>
    public interface IPermissionService
    {
        Task<bool> UserHasPermissionAsync(string userId, string moduleName, string permissionName);
        Task<bool> UserHasModuleAccessAsync(string userId, string moduleName);
        Task<List<string>> GetUserModulesAsync(string userId);
        Task<List<string>> GetUserPermissionsAsync(string userId, string moduleName);
        Task AssignPermissionToRoleAsync(string roleId, int modulePermissionId);
        Task RemovePermissionFromRoleAsync(string roleId, int modulePermissionId);
        Task<List<ModulePermission>> GetModulePermissionsAsync(string moduleName);
    }

    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public PermissionService(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico en un módulo
        /// </summary>
        public async Task<bool> UserHasPermissionAsync(string userId, string moduleName, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = _context.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id).ToList();

            var hasPermission = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Where(rp => rp.ModulePermission.ModuleName == moduleName)
                .Where(rp => rp.ModulePermission.PermissionName == permissionName)
                .Where(rp => rp.ModulePermission.IsActive)
                .AnyAsync();

            return hasPermission;
        }

        /// <summary>
        /// Verifica si un usuario tiene acceso a un módulo (al menos un permiso)
        /// </summary>
        public async Task<bool> UserHasModuleAccessAsync(string userId, string moduleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = _context.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id).ToList();

            var hasAccess = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Where(rp => rp.ModulePermission.ModuleName == moduleName)
                .Where(rp => rp.ModulePermission.IsActive)
                .AnyAsync();

            return hasAccess;
        }

        /// <summary>
        /// Obtiene la lista de módulos a los que un usuario tiene acceso
        /// </summary>
        public async Task<List<string>> GetUserModulesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = _context.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id).ToList();

            var modules = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Where(rp => rp.ModulePermission.IsActive)
                .Select(rp => rp.ModulePermission.ModuleName)
                .Distinct()
                .ToListAsync();

            return modules;
        }

        /// <summary>
        /// Obtiene los permisos de un usuario en un módulo específico
        /// </summary>
        public async Task<List<string>> GetUserPermissionsAsync(string userId, string moduleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = _context.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.Id).ToList();

            var permissions = await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Where(rp => rp.ModulePermission.ModuleName == moduleName)
                .Where(rp => rp.ModulePermission.IsActive)
                .Select(rp => rp.ModulePermission.PermissionName)
                .Distinct()
                .ToListAsync();

            return permissions;
        }

        /// <summary>
        /// Asigna un permiso a un rol
        /// </summary>
        public async Task AssignPermissionToRoleAsync(string roleId, int modulePermissionId)
        {
            var exists = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.ModulePermissionId == modulePermissionId);

            if (!exists)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    ModulePermissionId = modulePermissionId
                };

                _context.RolePermissions.Add(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Remueve un permiso de un rol
        /// </summary>
        public async Task RemovePermissionFromRoleAsync(string roleId, int modulePermissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.ModulePermissionId == modulePermissionId);

            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Obtiene todos los permisos de un módulo
        /// </summary>
        public async Task<List<ModulePermission>> GetModulePermissionsAsync(string moduleName)
        {
            return await _context.Set<ModulePermission>()
                .Where(mp => mp.ModuleName == moduleName && mp.IsActive)
                .ToListAsync();
        }
    }
}
