using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly ILogger<MantenimientoController> _logger;

        public MantenimientoController(ILogger<MantenimientoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Equipos()
        {
            ViewData["Title"] = "Equipos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Equipos", null)
            };
            return View("~/Views/Modules/Equipos.cshtml");
        }

        public IActionResult OrdenesMantenimiento()
        {
            ViewData["Title"] = "Órdenes de Trabajo";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Órdenes de Trabajo", null)
            };
            return View("~/Views/Modules/OrdenesMantenimiento.cshtml");
        }

        public IActionResult ReportesMantenimiento()
        {
            ViewData["Title"] = "Reportes de Mantenimiento";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Reportes", null)
            };
            return View("~/Views/Modules/ReportesMantenimiento.cshtml");
        }

        public IActionResult MantenimientoPreventivo()
        {
            ViewData["Title"] = "Mantenimiento Preventivo";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Mantenimiento Preventivo", null)
            };
            return View("~/Views/Modules/MantenimientoPreventivo.cshtml");
        }

        public IActionResult MantenimientoCorrectivo()
        {
            ViewData["Title"] = "Mantenimiento Correctivo";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Mantenimiento Correctivo", null)
            };
            return View("~/Views/Modules/MantenimientoCorrectivo.cshtml");
        }

        public IActionResult ChecklistMantenimiento()
        {
            ViewData["Title"] = "Checklist";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Checklist", null)
            };
            return View("~/Views/Modules/ChecklistMantenimiento.cshtml");
        }

        public IActionResult HistoricoMantenimiento()
        {
            ViewData["Title"] = "Histórico de Mantenimiento";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Mantenimiento", Url.Action("Mantenimiento", "Modules")),
                ("Histórico", null)
            };
            return View("~/Views/Modules/HistoricoMantenimiento.cshtml");
        }
    }
}
