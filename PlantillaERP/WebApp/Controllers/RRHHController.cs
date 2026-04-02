using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class RRHHController : Controller
    {
        private readonly ILogger<RRHHController> _logger;

        public RRHHController(ILogger<RRHHController> logger)
        {
            _logger = logger;
        }

        public IActionResult Empleados()
        {
            ViewData["Title"] = "Empleados";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Empleados", null)
            };
            return View("~/Views/Modules/Empleados.cshtml");
        }

        public IActionResult Nomina()
        {
            ViewData["Title"] = "Nómina";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Nómina", null)
            };
            return View("~/Views/Modules/Nomina.cshtml");
        }

        public IActionResult Capacitacion()
        {
            ViewData["Title"] = "Capacitación";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Capacitación", null)
            };
            return View("~/Views/Modules/Capacitacion.cshtml");
        }

        public IActionResult Evaluaciones()
        {
            ViewData["Title"] = "Evaluaciones";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Evaluaciones", null)
            };
            return View("~/Views/Modules/Evaluaciones.cshtml");
        }

        public IActionResult Beneficios()
        {
            ViewData["Title"] = "Beneficios";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Beneficios", null)
            };
            return View("~/Views/Modules/Beneficios.cshtml");
        }

        public IActionResult Asistencia()
        {
            ViewData["Title"] = "Asistencia";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Asistencia", null)
            };
            return View("~/Views/Modules/Asistencia.cshtml");
        }

        public IActionResult PermisosVacaciones()
        {
            ViewData["Title"] = "Permisos y Vacaciones";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Permisos y Vacaciones", null)
            };
            return View("~/Views/Modules/PermisosVacaciones.cshtml");
        }

        public IActionResult ReportesRRHH()
        {
            ViewData["Title"] = "Reportes RRHH";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Recursos Humanos", Url.Action("RRHH", "Modules")),
                ("Reportes RRHH", null)
            };
            return View("~/Views/Modules/ReportesRRHH.cshtml");
        }
    }
}
