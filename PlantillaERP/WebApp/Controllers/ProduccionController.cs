using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProduccionController : Controller
    {
        private readonly ILogger<ProduccionController> _logger;

        public ProduccionController(ILogger<ProduccionController> logger)
        {
            _logger = logger;
        }

        public IActionResult OrdenesProduccion()
        {
            ViewData["Title"] = "Órdenes de Producción";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Órdenes de Producción", null)
            };
            return View("~/Views/Modules/OrdenesProduccion.cshtml");
        }

        public IActionResult Recursos()
        {
            ViewData["Title"] = "Recursos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Recursos", null)
            };
            return View("~/Views/Modules/Recursos.cshtml");
        }

        public IActionResult ControlCalidad()
        {
            ViewData["Title"] = "Control de Calidad";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Control de Calidad", null)
            };
            return View("~/Views/Modules/ControlCalidad.cshtml");
        }

        public IActionResult Procesos()
        {
            ViewData["Title"] = "Procesos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Procesos", null)
            };
            return View("~/Views/Modules/Procesos.cshtml");
        }

        public IActionResult MaterialesProduccion()
        {
            ViewData["Title"] = "Materiales";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Materiales", null)
            };
            return View("~/Views/Modules/MaterialesProduccion.cshtml");
        }

        public IActionResult ProductosFinales()
        {
            ViewData["Title"] = "Productos Finales";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Productos Finales", null)
            };
            return View("~/Views/Modules/ProductosFinales.cshtml");
        }

        public IActionResult ReportesProduccion()
        {
            ViewData["Title"] = "Reportes de Producción";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Dashboard", Url.Action("Index", "Home")),
                ("Producción", Url.Action("Produccion", "Modules")),
                ("Reportes", null)
            };
            return View("~/Views/Modules/ReportesProduccion.cshtml");
        }
    }
}
