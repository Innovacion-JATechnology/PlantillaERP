namespace UserRoles.Identity.Constants
{
    /// <summary>
    /// Define los roles disponibles en el sistema
    /// </summary>
    public static class SystemRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Employee = "Employee";
        public const string Viewer = "Viewer";

        public static readonly List<string> AllRoles = new() 
        { 
            Admin, 
            Manager, 
            Employee, 
            Viewer 
        };
    }

    /// <summary>
    /// Define los módulos disponibles
    /// </summary>
    public static class ModuleNames
    {
        public const string Compras = "Compras";
        public const string Inventario = "Inventario";
        public const string Finanzas = "Finanzas";
        public const string Mantenimiento = "Mantenimiento";
        public const string Produccion = "Produccion";
        public const string RRHH = "RRHH";
        public const string Proyectos = "Proyectos";
        public const string Reporteador = "Reporteador";
        public const string Administracion = "Administracion";

        public static readonly List<string> AllModules = new()
        {
            Compras,
            Inventario,
            Finanzas,
            Mantenimiento,
            Produccion,
            RRHH,
            Proyectos,
            Reporteador,
            Administracion
        };
    }

    /// <summary>
    /// Define los permisos disponibles
    /// </summary>
    public static class PermissionNames
    {
        public const string View = "Ver";
        public const string Create = "Crear";
        public const string Edit = "Editar";
        public const string Delete = "Eliminar";
    }
}
