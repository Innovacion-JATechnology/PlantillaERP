using Microsoft.AspNetCore.Identity;

namespace UserRoles.Identity.Models
{
    /// <summary>
    /// Vincula roles con permisos de módulos
    /// </summary>
    public class RolePermission
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public int ModulePermissionId { get; set; }
        public ModulePermission ModulePermission { get; set; }
    }
}
