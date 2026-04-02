using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApp.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ILogger<ModulesController> _logger;

        public ModulesController(ILogger<ModulesController> logger)
        {
            _logger = logger;
        }

        // Compras Module
        public IActionResult Compras()
        {
            ViewData["Title"] = "Compras";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Compras Internacionales" }, { "Action", "ComprasInternacionales" }, { "Controller", "Compras" }, { "Icon", "fas fa-globe" }, { "Description", "Gestión de compras internacionales" } },
                new Dictionary<string, string> { { "Name", "Proveedores" }, { "Action", "Proveedores" }, { "Controller", "Compras" }, { "Icon", "fas fa-truck" }, { "Description", "Gestión de proveedores" } },
                new Dictionary<string, string> { { "Name", "Solicitudes" }, { "Action", "Solicitudes" }, { "Controller", "Compras" }, { "Icon", "fas fa-file-alt" }, { "Description", "Solicitudes de compra" } },
                new Dictionary<string, string> { { "Name", "Órdenes de Compra" }, { "Action", "OrdenesCompra" }, { "Controller", "Compras" }, { "Icon", "fas fa-shopping-cart" }, { "Description", "Órdenes de compra generadas" } },
                new Dictionary<string, string> { { "Name", "Reportes" }, { "Action", "ReportesCompras" }, { "Controller", "Compras" }, { "Icon", "fas fa-chart-bar" }, { "Description", "Reportes de compras" } },
                new Dictionary<string, string> { { "Name", "Contratos" }, { "Action", "Contratos" }, { "Controller", "Compras" }, { "Icon", "fas fa-file-contract" }, { "Description", "Gestión de contratos" } }
            };
            return View("ModuleView");
        }

        public IActionResult ComprasInternacionales()
        {
            ViewData["Title"] = "Compras Internacionales";
            return View();
        }

        // Inventario Module
        public IActionResult Inventario()
        {
            ViewData["Title"] = "Inventario";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Catálogo" }, { "Action", "Productos" }, { "Controller", "Inventario" }, { "Icon", "fas fa-shopping-bag" }, { "Description", "Gestión de productos" } },
                new Dictionary<string, string> { { "Name", "Stock" }, { "Action", "Stock" }, { "Controller", "Inventario" }, { "Icon", "fas fa-boxes" }, { "Description", "Control de inventario" } },
                new Dictionary<string, string> { { "Name", "Almacenes" }, { "Action", "Almacenes" }, { "Controller", "Inventario" }, { "Icon", "fas fa-warehouse" }, { "Description", "Gestión de almacenes" } },
                new Dictionary<string, string> { { "Name", "Categorías" }, { "Action", "Categorias" }, { "Controller", "Inventario" }, { "Icon", "fas fa-sitemap" }, { "Description", "Categorización de productos" } },
                new Dictionary<string, string> { { "Name", "Movimientos" }, { "Action", "Movimientos" }, { "Controller", "Inventario" }, { "Icon", "fas fa-arrow-right-arrow-left" }, { "Description", "Movimientos de inventario" } },
                new Dictionary<string, string> { { "Name", "Auditoría" }, { "Action", "AuditoriaInventario" }, { "Controller", "Inventario" }, { "Icon", "fas fa-history" }, { "Description", "Registro de cambios" } },
                new Dictionary<string, string> { { "Name", "Ajustes" }, { "Action", "AjustesInventario" }, { "Controller", "Inventario" }, { "Icon", "fas fa-sliders-h" }, { "Description", "Ajustes de inventario" } },
                new Dictionary<string, string> { { "Name", "Reportes" }, { "Action", "ReportesInventario" }, { "Controller", "Inventario" }, { "Icon", "fas fa-chart-pie" }, { "Description", "Reportes de inventario" } }
            };
            return View("ModuleView");
        }

        public IActionResult Productos()
        {
            ViewData["Title"] = "Productos";
            return View();
        }

        // Finanzas Module
        public IActionResult Finanzas()
        {
            ViewData["Title"] = "Finanzas";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Contabilidad" }, { "Action", "Contabilidad" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-book" }, { "Description", "Registros contables" } },
                new Dictionary<string, string> { { "Name", "Facturas" }, { "Action", "Facturas" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-receipt" }, { "Description", "Gestión de facturas" } },
                new Dictionary<string, string> { { "Name", "Reportes Financieros" }, { "Action", "ReportesFinancieros" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-chart-bar" }, { "Description", "Análisis financiero" } },
                new Dictionary<string, string> { { "Name", "Presupuestos" }, { "Action", "Presupuestos" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-calculator" }, { "Description", "Gestión de presupuestos" } },
                new Dictionary<string, string> { { "Name", "Cuentas por Pagar" }, { "Action", "CuentasPorPagar" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-credit-card" }, { "Description", "Deudas por pagar" } },
                new Dictionary<string, string> { { "Name", "Cuentas por Cobrar" }, { "Action", "CuentasPorCobrar" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-money-bills" }, { "Description", "Cuentas pendientes" } },
                new Dictionary<string, string> { { "Name", "Conciliaciones" }, { "Action", "Conciliaciones" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-check-double" }, { "Description", "Conciliación bancaria" } },
                new Dictionary<string, string> { { "Name", "Flujo de Caja" }, { "Action", "FlujoCaja" }, { "Controller", "Finanzas" }, { "Icon", "fas fa-water" }, { "Description", "Análisis de flujo" } }
            };
            return View("ModuleView");
        }

        // Mantenimiento Module
        public IActionResult Mantenimiento()
        {
            ViewData["Title"] = "Mantenimiento";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Equipos" }, { "Action", "Equipos" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-tools" }, { "Description", "Gestión de equipos" } },
                new Dictionary<string, string> { { "Name", "Órdenes de Trabajo" }, { "Action", "OrdenesMantenimiento" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-list-check" }, { "Description", "Órdenes de mantenimiento" } },
                new Dictionary<string, string> { { "Name", "Reportes" }, { "Action", "ReportesMantenimiento" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-wrench" }, { "Description", "Reportes de mantenimiento" } },
                new Dictionary<string, string> { { "Name", "Mantenimiento Preventivo" }, { "Action", "MantenimientoPreventivo" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-shield-alt" }, { "Description", "Tareas preventivas" } },
                new Dictionary<string, string> { { "Name", "Mantenimiento Correctivo" }, { "Action", "MantenimientoCorrectivo" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-hammer" }, { "Description", "Reparaciones" } },
                new Dictionary<string, string> { { "Name", "Checklist" }, { "Action", "ChecklistMantenimiento" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-clipboard-check" }, { "Description", "Listas de verificación" } },
                new Dictionary<string, string> { { "Name", "Histórico" }, { "Action", "HistoricoMantenimiento" }, { "Controller", "Mantenimiento" }, { "Icon", "fas fa-archive" }, { "Description", "Historial de mantenimiento" } }
            };
            return View("ModuleView");
        }

        // Produccion Module
        public IActionResult Produccion()
        {
            ViewData["Title"] = "Produccion";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Produccion", Url.Action("Produccion", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Órdenes de Producción" }, { "Action", "OrdenesProduccion" }, { "Controller", "Produccion" }, { "Icon", "fas fa-industry" }, { "Description", "Gestión de órdenes" } },
                new Dictionary<string, string> { { "Name", "Recursos" }, { "Action", "Recursos" }, { "Controller", "Produccion" }, { "Icon", "fas fa-cog" }, { "Description", "Recursos de producción" } },
                new Dictionary<string, string> { { "Name", "Control de Calidad" }, { "Action", "ControlCalidad" }, { "Controller", "Produccion" }, { "Icon", "fas fa-check-circle" }, { "Description", "Aseguramiento de calidad" } },
                new Dictionary<string, string> { { "Name", "Procesos" }, { "Action", "Procesos" }, { "Controller", "Produccion" }, { "Icon", "fas fa-project-diagram" }, { "Description", "Definición de procesos" } },
                new Dictionary<string, string> { { "Name", "Materiales" }, { "Action", "MaterialesProduccion" }, { "Controller", "Produccion" }, { "Icon", "fas fa-cube" }, { "Description", "Gestión de materiales" } },
                new Dictionary<string, string> { { "Name", "Productos Finales" }, { "Action", "ProductosFinales" }, { "Controller", "Produccion" }, { "Icon", "fas fa-box-open" }, { "Description", "Productos terminados" } },
                new Dictionary<string, string> { { "Name", "Reportes" }, { "Action", "ReportesProduccion" }, { "Controller", "Produccion" }, { "Icon", "fas fa-chart-line" }, { "Description", "Reportes de producción" } }
            };
            return View("ModuleView");
        }

        // RRHH Module
        public IActionResult RRHH()
        {
            ViewData["Title"] = "Recursos Humanos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Empleados" }, { "Action", "Empleados" }, { "Controller", "RRHH" }, { "Icon", "fas fa-user-tie" }, { "Description", "Gestión de empleados" } },
                new Dictionary<string, string> { { "Name", "Nómina" }, { "Action", "Nomina" }, { "Controller", "RRHH" }, { "Icon", "fas fa-money-check" }, { "Description", "Procesamiento de nómina" } },
                new Dictionary<string, string> { { "Name", "Capacitación" }, { "Action", "Capacitacion" }, { "Controller", "RRHH" }, { "Icon", "fas fa-graduation-cap" }, { "Description", "Programas de capacitación" } },
                new Dictionary<string, string> { { "Name", "Evaluaciones" }, { "Action", "Evaluaciones" }, { "Controller", "RRHH" }, { "Icon", "fas fa-star" }, { "Description", "Evaluaciones del desempeño" } },
                new Dictionary<string, string> { { "Name", "Beneficios" }, { "Action", "Beneficios" }, { "Controller", "RRHH" }, { "Icon", "fas fa-gift" }, { "Description", "Gestión de beneficios" } },
                new Dictionary<string, string> { { "Name", "Asistencia" }, { "Action", "Asistencia" }, { "Controller", "RRHH" }, { "Icon", "fas fa-clipboard-list" }, { "Description", "Control de asistencia" } },
                new Dictionary<string, string> { { "Name", "Permisos y Vacaciones" }, { "Action", "PermisosVacaciones" }, { "Controller", "RRHH" }, { "Icon", "fas fa-calendar" }, { "Description", "Solicitud de permisos" } },
                new Dictionary<string, string> { { "Name", "Reportes RRHH" }, { "Action", "ReportesRRHH" }, { "Controller", "RRHH" }, { "Icon", "fas fa-users-chart" }, { "Description", "Reportes de personal" } }
            };
            return View("ModuleView");
        }

        // Proyectos Module
        public IActionResult Proyectos()
        {
            ViewData["Title"] = "Proyectos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Proyectos" }, { "Action", "ListaProyectos" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-briefcase" }, { "Description", "Lista de proyectos" } },
                new Dictionary<string, string> { { "Name", "Tareas" }, { "Action", "Tareas" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-tasks" }, { "Description", "Gestión de tareas" } },
                new Dictionary<string, string> { { "Name", "Recursos" }, { "Action", "RecursosProyecto" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-users-cog" }, { "Description", "Asignación de recursos" } },
                new Dictionary<string, string> { { "Name", "Diagrama Gantt" }, { "Action", "DiagramaGantt" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-chart-gantt" }, { "Description", "Visualización de cronograma" } },
                new Dictionary<string, string> { { "Name", "Presupuesto" }, { "Action", "PresupuestoProyectos" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-coins" }, { "Description", "Gestión presupuestaria" } },
                new Dictionary<string, string> { { "Name", "Documentos" }, { "Action", "DocumentosProyectos" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-file-pdf" }, { "Description", "Documentación del proyecto" } },
                new Dictionary<string, string> { { "Name", "Reportes" }, { "Action", "ReportesProyectos" }, { "Controller", "Proyectos" }, { "Icon", "fas fa-file-chart-line" }, { "Description", "Reportes de proyectos" } }
            };
            return View("ModuleView");
        }

        // Reporteador Module
        public IActionResult Reporteador()
        {
            ViewData["Title"] = "Reporteador";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Reportes Predefinidos" }, { "Action", "ReportesPredefinidos" }, { "Controller", "Reporteador" }, { "Icon", "fas fa-print" }, { "Description", "Reportes del sistema" } },
                new Dictionary<string, string> { { "Name", "Crear Reporte" }, { "Action", "CrearReporte" }, { "Controller", "Reporteador" }, { "Icon", "fas fa-file-csv" }, { "Description", "Diseñador de reportes" } },
                new Dictionary<string, string> { { "Name", "Programación" }, { "Action", "ProgramacionReportes" }, { "Controller", "Reporteador" }, { "Icon", "fas fa-calendar-alt" }, { "Description", "Programar generación" } },
                new Dictionary<string, string> { { "Name", "Plantillas" }, { "Action", "PlantillasReportes" }, { "Controller", "Reporteador" }, { "Icon", "fas fa-file-template" }, { "Description", "Plantillas disponibles" } },
                new Dictionary<string, string> { { "Name", "Historial" }, { "Action", "HistorialReportes" }, { "Controller", "Reporteador" }, { "Icon", "fas fa-history" }, { "Description", "Historial de reportes" } },
                new Dictionary<string, string> { { "Name", "Exportar" }, { "Action", "ExportarReportes" }, { "Controller", "Reporteador" }, { "Icon", "fas fa-download" }, { "Description", "Exportar reportes" } }
            };
            return View("ModuleView");
        }

        // Administracion Module
        public IActionResult Administracion()
        {
            ViewData["Title"] = "Administración";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Administración", Url.Action("Administracion", "Modules"))
            };
            ViewData["ModuleOptions"] = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "Name", "Usuarios" }, { "Action", "Usuarios" }, { "Controller", "Administracion" }, { "Icon", "fas fa-users" }, { "Description", "Gestión de usuarios" } },
                new Dictionary<string, string> { { "Name", "Roles y Permisos" }, { "Action", "RolesPermisos" }, { "Controller", "Administracion" }, { "Icon", "fas fa-lock" }, { "Description", "Control de acceso" } },
                new Dictionary<string, string> { { "Name", "Configuración" }, { "Action", "Configuracion" }, { "Controller", "Administracion" }, { "Icon", "fas fa-cog" }, { "Description", "Parámetros del sistema" } },
                new Dictionary<string, string> { { "Name", "Auditoría" }, { "Action", "Auditoria" }, { "Controller", "Administracion" }, { "Icon", "fas fa-eye" }, { "Description", "Registro de auditoría" } },
                new Dictionary<string, string> { { "Name", "Respaldos" }, { "Action", "Respaldos" }, { "Controller", "Administracion" }, { "Icon", "fas fa-database" }, { "Description", "Copias de seguridad" } },
                new Dictionary<string, string> { { "Name", "Parámetros Generales" }, { "Action", "ParametrosGenerales" }, { "Controller", "Administracion" }, { "Icon", "fas fa-sliders-h" }, { "Description", "Configuración general" } },
                new Dictionary<string, string> { { "Name", "Logs" }, { "Action", "Logs" }, { "Controller", "Administracion" }, { "Icon", "fas fa-file-lines" }, { "Description", "Registro de eventos" } },
                new Dictionary<string, string> { { "Name", "Integraciones" }, { "Action", "Integraciones" }, { "Controller", "Administracion" }, { "Icon", "fas fa-plug" }, { "Description", "Integraciones del sistema" } }
            };
            return View("ModuleView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private class ErrorViewModel
        {
            public string? RequestId { get; set; }
            public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }
    }
}
