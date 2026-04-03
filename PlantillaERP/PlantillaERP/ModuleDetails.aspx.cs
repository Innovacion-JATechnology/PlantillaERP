using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace PlantillaERP
{
    public partial class ModuleDetails : Page
    {
        private class SubModuleOption
        {
            public string Title { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public string Target { get; set; }
            public string Description { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string module = Request.QueryString["module"];
                
                if (string.IsNullOrEmpty(module))
                {
                    Response.Redirect("/");
                    return;
                }

                litModuleTitle.Text = GetModuleTitle(module);
                GenerateSubmodules(module);
            }
        }

        private string GetModuleTitle(string module)
        {
            switch (module.ToLower())
            {
                case "inventario": return "Inventario";
                case "compras": return "Compras";
                case "finanzas": return "Finanzas";
                case "mantenimiento": return "Mantenimiento";
                case "produccion": return "Producción";
                case "rrhh": return "RRHH";
                case "proyectos": return "Proyectos";
                case "reporteador": return "Reporteador";
                case "administracion": return "Administración";
                default: return "Módulo";
            }
        }

        private void GenerateSubmodules(string module)
        {
            List<SubModuleOption> options = GetSubmoduleOptions(module);

            foreach (var option in options)
            {
                HtmlAnchor link = new HtmlAnchor();
                link.HRef = option.Url;
                link.Attributes["class"] = "module-card";
                link.Target = option.Target;

                // Icono
                HtmlGenericControl iconDiv = new HtmlGenericControl("div");
                iconDiv.Attributes["class"] = "module-icon";
                HtmlGenericControl icon = new HtmlGenericControl("i");
                icon.Attributes["class"] = option.Icon;
                iconDiv.Controls.Add(icon);

                // Contenido
                HtmlGenericControl contentDiv = new HtmlGenericControl("div");
                contentDiv.Attributes["class"] = "module-content";

                HtmlGenericControl title = new HtmlGenericControl("h3");
                title.Attributes["class"] = "module-title";
                title.InnerText = option.Title;

                HtmlGenericControl description = new HtmlGenericControl("p");
                description.Attributes["class"] = "module-description";
                description.InnerText = option.Description;

                contentDiv.Controls.Add(title);
                contentDiv.Controls.Add(description);

                link.Controls.Add(iconDiv);
                link.Controls.Add(contentDiv);

                submodulesGrid.Controls.Add(link);
            }
        }

        private List<SubModuleOption> GetSubmoduleOptions(string module)
        {
            var options = new List<SubModuleOption>();

            switch (module.ToLower())
            {
                case "inventario":
                    options.Add(new SubModuleOption { Title = "Catálogo", Icon = "fas fa-box", Url = "/Inventario/Productos", Target = "_self", Description = "Gestión de productos y SKU" });
                    options.Add(new SubModuleOption { Title = "Almacenes", Icon = "fas fa-building", Url = "/Inventario/Almacenes", Target = "_self", Description = "Configuración de ubicaciones y almacenes" });
                    options.Add(new SubModuleOption { Title = "Existencias", Icon = "fas fa-archive", Url = "/Inventario/Existencias", Target = "_self", Description = "Niveles de stock y valuaciones" });
                    options.Add(new SubModuleOption { Title = "Movimientos", Icon = "fas fa-exchange-alt", Url = "/Inventario/Movimientos", Target = "_self", Description = "Movimientos internos y ajustes" });
                    options.Add(new SubModuleOption { Title = "Traspasos", Icon = "fas fa-shuffle", Url = "/Inventario/Traspasos", Target = "_self", Description = "Traslado entre almacenes" });
                    options.Add(new SubModuleOption { Title = "Conteos", Icon = "fas fa-list-check", Url = "/Inventario/Conteos", Target = "_self", Description = "Conteos cíclicos e inventarios físicos" });
                    break;

                case "compras":
                    options.Add(new SubModuleOption { Title = "Compras Internacionales", Icon = "fas fa-globe", Url = "/Compras/ComprasInternacionales", Target = "_self", Description = "Negociación y documentación de importaciones" });
                    options.Add(new SubModuleOption { Title = "Órdenes", Icon = "fas fa-shopping-bag", Url = "/Compras/Ordenes", Target = "_self", Description = "Creación y control de órdenes de compra" });
                    options.Add(new SubModuleOption { Title = "Proveedores", Icon = "fas fa-people-carry", Url = "/Compras/Proveedores", Target = "_self", Description = "Registro y evaluación de proveedores" });
                    options.Add(new SubModuleOption { Title = "Recepción", Icon = "fas fa-arrow-down", Url = "/Compras/Recepcion", Target = "_self", Description = "Recepción y verificación de mercancías" });
                    options.Add(new SubModuleOption { Title = "Devoluciones", Icon = "fas fa-arrow-left", Url = "/Compras/Devoluciones", Target = "_self", Description = "Gestión de devoluciones a proveedores" });
                    break;

                case "finanzas":
                    options.Add(new SubModuleOption { Title = "Contabilidad", Icon = "fas fa-book", Url = "/Finanzas/Contabilidad", Target = "_self", Description = "Registro de asientos y libros contables" });
                    options.Add(new SubModuleOption { Title = "CxP", Icon = "fas fa-credit-card", Url = "/Finanzas/CxP", Target = "_self", Description = "Gestión de cuentas por pagar y proveedores" });
                    options.Add(new SubModuleOption { Title = "CxC", Icon = "fas fa-money-bill", Url = "/Finanzas/CxC", Target = "_self", Description = "Gestión de cuentas por cobrar y clientes" });
                    options.Add(new SubModuleOption { Title = "Tesorería", Icon = "fas fa-university", Url = "/Finanzas/Tesoreria", Target = "_self", Description = "Movimientos bancarios y conciliaciones" });
                    options.Add(new SubModuleOption { Title = "Activos", Icon = "fas fa-industry", Url = "/Finanzas/Activos", Target = "_self", Description = "Control de activos fijos y depreciaciones" });
                    options.Add(new SubModuleOption { Title = "Presupuestos", Icon = "fas fa-chart-bar", Url = "/Finanzas/Presupuestos", Target = "_self", Description = "Planificación y control presupuestal" });
                    break;

                case "mantenimiento":
                    options.Add(new SubModuleOption { Title = "Maquinaria y Equipo", Icon = "fas fa-microchip", Url = "/Mantenimiento/Equipos", Target = "_self", Description = "Inventario y gestión de equipos" });
                    options.Add(new SubModuleOption { Title = "Preventivo", Icon = "fas fa-calendar-check", Url = "/Mantenimiento/Preventivo", Target = "_self", Description = "Programación de mantenimiento preventivo" });
                    options.Add(new SubModuleOption { Title = "Correctivo", Icon = "fas fa-tools", Url = "/Mantenimiento/Correctivo", Target = "_self", Description = "Gestión de órdenes correctivas" });
                    options.Add(new SubModuleOption { Title = "Taller", Icon = "fas fa-list-check", Url = "/Mantenimiento/Tareas", Target = "_self", Description = "Asignación y seguimiento de tareas de mantenimiento" });
                    options.Add(new SubModuleOption { Title = "Historial", Icon = "fas fa-history", Url = "/Mantenimiento/Historial", Target = "_self", Description = "Registro de intervenciones y servicios" });
                    options.Add(new SubModuleOption { Title = "Reportes", Icon = "fas fa-chart-line", Url = "/Analitica/Reportes", Target = "_self", Description = "Informes de cumplimiento y costos" });
                    break;

                case "produccion":
                    options.Add(new SubModuleOption { Title = "BOM", Icon = "fas fa-diagram-project", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Manejo de lista de materiales" });
                    options.Add(new SubModuleOption { Title = "Órdenes", Icon = "fas fa-cog", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Planificación y ejecución de órdenes de producción" });
                    options.Add(new SubModuleOption { Title = "Ruteo", Icon = "fas fa-route", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Definición de rutas y centros de trabajo" });
                    options.Add(new SubModuleOption { Title = "MRP", Icon = "fas fa-server", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Planificación de necesidades de materiales" });
                    options.Add(new SubModuleOption { Title = "Programación", Icon = "fas fa-calendar", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Calendario y asignación de capacidad" });
                    break;

                case "rrhh":
                    options.Add(new SubModuleOption { Title = "Empleados", Icon = "fas fa-user", Url = "/RRHH/Empleados", Target = "_self", Description = "Gestión de datos y contratos" });
                    options.Add(new SubModuleOption { Title = "Asistencias", Icon = "fas fa-clock", Url = "/RRHH/Asistencias", Target = "_self", Description = "Registro de entradas y salidas" });
                    options.Add(new SubModuleOption { Title = "Nómina", Icon = "fas fa-coins", Url = "/RRHH/Nomina", Target = "_self", Description = "Cálculo y pagos de salarios" });
                    options.Add(new SubModuleOption { Title = "Prestaciones", Icon = "fas fa-gift", Url = "/RRHH/Prestaciones", Target = "_self", Description = "Beneficios y prestaciones laborales" });
                    options.Add(new SubModuleOption { Title = "Evaluaciones", Icon = "fas fa-chart-bar", Url = "/RRHH/Evaluaciones", Target = "_self", Description = "Evaluación de desempeño y KPIs" });
                    break;

                case "proyectos":
                    options.Add(new SubModuleOption { Title = "Proyectos", Icon = "fas fa-folder", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Gestión de portafolio y fases" });
                    options.Add(new SubModuleOption { Title = "Tareas", Icon = "fas fa-tasks", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Asignación y seguimiento de tareas" });
                    options.Add(new SubModuleOption { Title = "Tiempo", Icon = "fas fa-hourglass-end", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Registro de tiempos y productividad" });
                    options.Add(new SubModuleOption { Title = "Costos", Icon = "fas fa-dollar-sign", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Control de costos por proyecto" });
                    options.Add(new SubModuleOption { Title = "Facturación", Icon = "fas fa-receipt", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Facturación vinculada a proyectos" });
                    break;

                case "reporteador":
                    options.Add(new SubModuleOption { Title = "Inicios", Icon = "fas fa-tachometer-alt", Url = "/Analitica/Reportes", Target = "_self", Description = "Visualización interactiva de métricas" });
                    options.Add(new SubModuleOption { Title = "KPIs", Icon = "fas fa-chart-line", Url = "/Analitica/Reportes", Target = "_self", Description = "Indicadores clave de rendimiento" });
                    options.Add(new SubModuleOption { Title = "Reportes", Icon = "fas fa-file-pdf", Url = "/Analitica/Reportes", Target = "_self", Description = "Generación y programación de reportes" });
                    options.Add(new SubModuleOption { Title = "Exportación", Icon = "fas fa-download", Url = "/EnConstruccion.aspx", Target = "_self", Description = "Exportación de datos a formatos comunes" });
                    break;

                case "administracion":
                    options.Add(new SubModuleOption { Title = "Usuarios", Icon = "fas fa-user-cog", Url = "/Admin/Usuarios", Target = "_self", Description = "Gestión de cuentas y permisos" });
                    options.Add(new SubModuleOption { Title = "Roles", Icon = "fas fa-shield-alt", Url = "/Admin/Roles", Target = "_self", Description = "Definición de roles y perfiles" });
                    options.Add(new SubModuleOption { Title = "Configuración", Icon = "fas fa-sliders-h", Url = "/Admin/Configuracion", Target = "_self", Description = "Parámetros y preferencias del sistema" });
                    options.Add(new SubModuleOption { Title = "Integraciones", Icon = "fas fa-plug", Url = "/Admin/Integraciones", Target = "_self", Description = "Conexiones con servicios externos" });
                    options.Add(new SubModuleOption { Title = "Logs", Icon = "fas fa-file-code", Url = "/Admin/Logs", Target = "_self", Description = "Registro de auditoría y eventos" });
                    break;

                default:
                    options.Add(new SubModuleOption { Title = "No hay subopciones", Icon = "fas fa-circle-x", Url = "/", Target = "_self", Description = "Módulo no encontrado" });
                    break;
            }

            return options;
        }
    }
}
