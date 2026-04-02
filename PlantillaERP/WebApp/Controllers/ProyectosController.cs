using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly ILogger<ProyectosController> _logger;

        public ProyectosController(ILogger<ProyectosController> logger)
        {
            _logger = logger;
        }

        public IActionResult ListaProyectos()
        {
            ViewData["Title"] = "Proyectos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Proyectos", null)
            };
            return View("~/Views/Modules/ListaProyectos.cshtml");
        }

        public IActionResult Tareas()
        {
            ViewData["Title"] = "Tareas";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Tareas", null)
            };
            return View("~/Views/Modules/Tareas.cshtml");
        }

        public IActionResult RecursosProyecto()
        {
            ViewData["Title"] = "Recursos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Recursos", null)
            };
            return View("~/Views/Modules/RecursosProyecto.cshtml");
        }

        public IActionResult DiagramaGantt()
        {
            ViewData["Title"] = "Diagrama Gantt";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Diagrama Gantt", null)
            };
            return View("~/Views/Modules/DiagramaGantt.cshtml");
        }

        public IActionResult PresupuestoProyectos()
        {
            ViewData["Title"] = "Presupuesto";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Presupuesto", null)
            };
            return View("~/Views/Modules/PresupuestoProyectos.cshtml");
        }

        public IActionResult DocumentosProyectos()
        {
            ViewData["Title"] = "Documentos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Documentos", null)
            };
            return View("~/Views/Modules/DocumentosProyectos.cshtml");
        }

        public IActionResult ReportesProyectos()
        {
            ViewData["Title"] = "Reportes";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Proyectos", Url.Action("Proyectos", "Modules")),
                ("Reportes", null)
            };
            return View("~/Views/Modules/ReportesProyectos.cshtml");
        }
    }
}
