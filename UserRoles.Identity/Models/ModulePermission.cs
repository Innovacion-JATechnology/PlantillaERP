namespace UserRoles.Identity.Models
{
    /// <summary>
    /// Define los permisos por módulo en el ERP
    /// </summary>
    public class ModulePermission
    {
        public int Id { get; set; }
        public string ModuleName { get; set; } // "Compras", "Inventario", etc.
        public string PermissionName { get; set; } // "Ver", "Crear", "Editar", "Eliminar"
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
