using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ReporteadorController : Controller
    {
        private readonly ILogger<ReporteadorController> _logger;

        public ReporteadorController(ILogger<ReporteadorController> logger)
        {
            _logger = logger;
        }

        public IActionResult ReportesPredefinidos()
        {
            ViewData["Title"] = "Reportes Predefinidos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules")),
                ("Reportes Predefinidos", null)
            };
            return View("~/Views/Modules/ReportesPredefinidos.cshtml");
        }

        public IActionResult CrearReporte()
        {
            ViewData["Title"] = "Crear Reporte";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules")),
                ("Crear Reporte", null)
            };
            return View("~/Views/Modules/CrearReporte.cshtml");
        }

        public IActionResult ProgramacionReportes()
        {
            ViewData["Title"] = "Programación";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules")),
                ("Programación", null)
            };
            return View("~/Views/Modules/ProgramacionReportes.cshtml");
        }

        public IActionResult PlantillasReportes()
        {
            ViewData["Title"] = "Plantillas";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules")),
                ("Plantillas", null)
            };
            return View("~/Views/Modules/PlantillasReportes.cshtml");
        }

        public IActionResult HistorialReportes()
        {
            ViewData["Title"] = "Historial";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules")),
                ("Historial", null)
            };
            return View("~/Views/Modules/HistorialReportes.cshtml");
        }

        public IActionResult ExportarReportes()
        {
            ViewData["Title"] = "Exportar";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Reporteador", Url.Action("Reporteador", "Modules")),
                ("Exportar", null)
            };
            return View("~/Views/Modules/ExportarReportes.cshtml");
        }
    }
}
