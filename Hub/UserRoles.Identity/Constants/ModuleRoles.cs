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

    /// <summary>
    /// Define los submódulos disponibles dentro de cada módulo
    /// </summary>
    public static class SubmoduleNames
    {
        // Compras submodules
        public const string ComprasInternacionales = "Compras.ComprasInternacionales";
        public const string Proveedores = "Compras.Proveedores";
        public const string Solicitudes = "Compras.Solicitudes";
        public const string OrdenesCompra = "Compras.OrdenesCompra";
        public const string ReportesCompras = "Compras.ReportesCompras";
        public const string Contratos = "Compras.Contratos";

        // Inventario submodules
        public const string Catalogo = "Inventario.Catalogo";
        public const string Stock = "Inventario.Stock";
        public const string Almacenes = "Inventario.Almacenes";
        public const string Categorias = "Inventario.Categorias";
        public const string Movimientos = "Inventario.Movimientos";
        public const string AuditoriaInventario = "Inventario.AuditoriaInventario";
        public const string AjustesInventario = "Inventario.AjustesInventario";
        public const string ReportesInventario = "Inventario.ReportesInventario";

        // Finanzas submodules
        public const string Contabilidad = "Finanzas.Contabilidad";
        public const string Facturas = "Finanzas.Facturas";
        public const string ReportesFinancieros = "Finanzas.ReportesFinancieros";
        public const string Presupuestos = "Finanzas.Presupuestos";
        public const string CuentasPorPagar = "Finanzas.CuentasPorPagar";
        public const string CuentasPorCobrar = "Finanzas.CuentasPorCobrar";
        public const string Conciliaciones = "Finanzas.Conciliaciones";
        public const string FlujoCaja = "Finanzas.FlujoCaja";

        // Mantenimiento submodules
        public const string Equipos = "Mantenimiento.Equipos";
        public const string OrdenesMantenimiento = "Mantenimiento.OrdenesMantenimiento";
        public const string ReportesMantenimiento = "Mantenimiento.ReportesMantenimiento";
        public const string MantenimientoPreventivo = "Mantenimiento.MantenimientoPreventivo";
        public const string MantenimientoCorrectivo = "Mantenimiento.MantenimientoCorrectivo";
        public const string ChecklistMantenimiento = "Mantenimiento.ChecklistMantenimiento";
        public const string HistoricoMantenimiento = "Mantenimiento.HistoricoMantenimiento";

        // Produccion submodules
        public const string OrdenesProduccion = "Produccion.OrdenesProduccion";
        public const string Recursos = "Produccion.Recursos";
        public const string ControlCalidad = "Produccion.ControlCalidad";
        public const string Procesos = "Produccion.Procesos";
        public const string MaterialesProduccion = "Produccion.MaterialesProduccion";
        public const string ProductosFinales = "Produccion.ProductosFinales";
        public const string ReportesProduccion = "Produccion.ReportesProduccion";

        // RRHH submodules
        public const string Empleados = "RRHH.Empleados";
        public const string Nomina = "RRHH.Nomina";
        public const string Capacitacion = "RRHH.Capacitacion";
        public const string Evaluaciones = "RRHH.Evaluaciones";
        public const string Beneficios = "RRHH.Beneficios";
        public const string Asistencia = "RRHH.Asistencia";
        public const string PermisosVacaciones = "RRHH.PermisosVacaciones";
        public const string ReportesRRHH = "RRHH.ReportesRRHH";

        // Proyectos submodules
        public const string ListaProyectos = "Proyectos.ListaProyectos";
        public const string Tareas = "Proyectos.Tareas";
        public const string RecursosProyecto = "Proyectos.RecursosProyecto";
        public const string DiagramaGantt = "Proyectos.DiagramaGantt";
        public const string PresupuestoProyectos = "Proyectos.PresupuestoProyectos";
        public const string DocumentosProyectos = "Proyectos.DocumentosProyectos";
        public const string ReportesProyectos = "Proyectos.ReportesProyectos";
    }
}
