using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class FinanzasController : Controller
    {
        private readonly ILogger<FinanzasController> _logger;

        public FinanzasController(ILogger<FinanzasController> logger)
        {
            _logger = logger;
        }

        public IActionResult Contabilidad()
        {
            ViewData["Title"] = "Contabilidad";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Contabilidad", null)
            };
            return View("~/Views/Modules/Contabilidad.cshtml");
        }

        public IActionResult Facturas()
        {
            ViewData["Title"] = "Facturas";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Facturas", null)
            };
            return View("~/Views/Modules/Facturas.cshtml");
        }

        public IActionResult ReportesFinancieros()
        {
            ViewData["Title"] = "Reportes Financieros";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Reportes Financieros", null)
            };
            return View("~/Views/Modules/ReportesFinancieros.cshtml");
        }

        public IActionResult Presupuestos()
        {
            ViewData["Title"] = "Presupuestos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Presupuestos", null)
            };
            return View("~/Views/Modules/Presupuestos.cshtml");
        }

        public IActionResult CuentasPorPagar()
        {
            ViewData["Title"] = "Cuentas por Pagar";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Cuentas por Pagar", null)
            };
            return View("~/Views/Modules/CuentasPorPagar.cshtml");
        }

        public IActionResult CuentasPorCobrar()
        {
            ViewData["Title"] = "Cuentas por Cobrar";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Cuentas por Cobrar", null)
            };
            return View("~/Views/Modules/CuentasPorCobrar.cshtml");
        }

        public IActionResult Conciliaciones()
        {
            ViewData["Title"] = "Conciliaciones";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Conciliaciones", null)
            };
            return View("~/Views/Modules/Conciliaciones.cshtml");
        }

        public IActionResult FlujoCaja()
        {
            ViewData["Title"] = "Flujo de Caja";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Finanzas", Url.Action("Finanzas", "Modules")),
                ("Flujo de Caja", null)
            };
            return View("~/Views/Modules/FlujoCaja.cshtml");
        }
    }
}
