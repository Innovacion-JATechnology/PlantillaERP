using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        public IActionResult ColorSettings()
        {
            ViewData["Title"] = "Configuración de Colores";
            return View();
        }

        [HttpPost]
        public IActionResult SaveColors([FromBody] ColorSettingsModel settings)
        {
            try
            {
                if (settings == null)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                // Here you would save the colors to database or configuration
                // For now, we'll just return success
                return Json(new { success = true, message = "Colores guardados correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving color settings");
                return Json(new { success = false, message = "Error al guardar los colores" });
            }
        }

        [HttpPost]
        public IActionResult ResetColors()
        {
            try
            {
                // Reset colors to default values
                return Json(new { success = true, message = "Colores restablecidos a valores predeterminados" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting color settings");
                return Json(new { success = false, message = "Error al restablecer los colores" });
            }
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

    public class ColorSettingsModel
    {
        // Navbar colors
        public int NavbarColor1R { get; set; } = 87;
        public int NavbarColor1G { get; set; } = 179;
        public int NavbarColor1B { get; set; } = 252;
        public int NavbarColor2R { get; set; } = 165;
        public int NavbarColor2G { get; set; } = 95;
        public int NavbarColor2B { get; set; } = 253;
        public int NavbarColor3R { get; set; } = 16;
        public int NavbarColor3G { get; set; } = 55;
        public int NavbarColor3B { get; set; } = 161;

        // Sidebar colors
        public int SidebarColor1R { get; set; } = 87;
        public int SidebarColor1G { get; set; } = 179;
        public int SidebarColor1B { get; set; } = 252;
        public int SidebarColor2R { get; set; } = 130;
        public int SidebarColor2G { get; set; } = 157;
        public int SidebarColor2B { get; set; } = 245;

        // Module colors
        public int ModuleColor1R { get; set; } = 87;
        public int ModuleColor1G { get; set; } = 179;
        public int ModuleColor1B { get; set; } = 252;
        public int ModuleColor2R { get; set; } = 165;
        public int ModuleColor2G { get; set; } = 95;
        public int ModuleColor2B { get; set; } = 253;
    }
}
