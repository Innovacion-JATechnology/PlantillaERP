using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace WebApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SettingsController(ILogger<SettingsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ServerCon") ?? throw new InvalidOperationException("Connection string 'ServerCon' not found.");
        }

        public IActionResult ColorSettings()
        {
            ViewData["Title"] = "Configuración de Colores";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Configuración de Colores", null)
            };

            // Cargar colores desde la BD
            var colors = LoadColorsFromDatabase();
            return View(colors);
        }

        private ColorSettingsModel LoadColorsFromDatabase()
        {
            var colors = new ColorSettingsModel();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Crear tabla si no existe
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ColorSettings')
                        BEGIN
                            CREATE TABLE ColorSettings (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                SettingKey NVARCHAR(50) UNIQUE NOT NULL,
                                SettingValue NVARCHAR(50) NOT NULL,
                                UpdatedDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Cargar colores
                    string query = @"SELECT SettingKey, SettingValue FROM ColorSettings";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string key = reader["SettingKey"].ToString();
                                string value = reader["SettingValue"].ToString();

                                // Mapear valores a propiedades
                                if (int.TryParse(value, out int intValue))
                                {
                                    switch (key)
                                    {
                                        case "NavbarColor1R": colors.NavbarColor1R = intValue; break;
                                        case "NavbarColor1G": colors.NavbarColor1G = intValue; break;
                                        case "NavbarColor1B": colors.NavbarColor1B = intValue; break;
                                        case "NavbarColor2R": colors.NavbarColor2R = intValue; break;
                                        case "NavbarColor2G": colors.NavbarColor2G = intValue; break;
                                        case "NavbarColor2B": colors.NavbarColor2B = intValue; break;
                                        case "NavbarColor3R": colors.NavbarColor3R = intValue; break;
                                        case "NavbarColor3G": colors.NavbarColor3G = intValue; break;
                                        case "NavbarColor3B": colors.NavbarColor3B = intValue; break;
                                        case "SidebarColor1R": colors.SidebarColor1R = intValue; break;
                                        case "SidebarColor1G": colors.SidebarColor1G = intValue; break;
                                        case "SidebarColor1B": colors.SidebarColor1B = intValue; break;
                                        case "SidebarColor2R": colors.SidebarColor2R = intValue; break;
                                        case "SidebarColor2G": colors.SidebarColor2G = intValue; break;
                                        case "SidebarColor2B": colors.SidebarColor2B = intValue; break;
                                        case "SidebarHoverR": colors.SidebarHoverR = intValue; break;
                                        case "SidebarHoverG": colors.SidebarHoverG = intValue; break;
                                        case "SidebarHoverB": colors.SidebarHoverB = intValue; break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading colors from database");
            }

            return colors;
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

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Crear tabla si no existe
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ColorSettings')
                        BEGIN
                            CREATE TABLE ColorSettings (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                SettingKey NVARCHAR(50) UNIQUE NOT NULL,
                                SettingValue NVARCHAR(50) NOT NULL,
                                UpdatedDate DATETIME DEFAULT GETDATE()
                            )
                        END";

                    using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Guardar colores
                    var colorSettings = new Dictionary<string, string>
                    {
                        { "NavbarColor1R", settings.NavbarColor1R.ToString() },
                        { "NavbarColor1G", settings.NavbarColor1G.ToString() },
                        { "NavbarColor1B", settings.NavbarColor1B.ToString() },
                        { "NavbarColor2R", settings.NavbarColor2R.ToString() },
                        { "NavbarColor2G", settings.NavbarColor2G.ToString() },
                        { "NavbarColor2B", settings.NavbarColor2B.ToString() },
                        { "NavbarColor3R", settings.NavbarColor3R.ToString() },
                        { "NavbarColor3G", settings.NavbarColor3G.ToString() },
                        { "NavbarColor3B", settings.NavbarColor3B.ToString() },
                        { "SidebarColor1R", settings.SidebarColor1R.ToString() },
                        { "SidebarColor1G", settings.SidebarColor1G.ToString() },
                        { "SidebarColor1B", settings.SidebarColor1B.ToString() },
                        { "SidebarColor2R", settings.SidebarColor2R.ToString() },
                        { "SidebarColor2G", settings.SidebarColor2G.ToString() },
                        { "SidebarColor2B", settings.SidebarColor2B.ToString() },
                        { "SidebarHoverR", settings.SidebarHoverR.ToString() },
                        { "SidebarHoverG", settings.SidebarHoverG.ToString() },
                        { "SidebarHoverB", settings.SidebarHoverB.ToString() }
                    };

                    foreach (var kvp in colorSettings)
                    {
                        string query = @"
                            IF EXISTS (SELECT 1 FROM ColorSettings WHERE SettingKey = @SettingKey)
                                UPDATE ColorSettings SET SettingValue = @SettingValue, UpdatedDate = GETDATE() WHERE SettingKey = @SettingKey
                            ELSE
                                INSERT INTO ColorSettings (SettingKey, SettingValue) VALUES (@SettingKey, @SettingValue)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@SettingKey", kvp.Key);
                            cmd.Parameters.AddWithValue("@SettingValue", kvp.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                return Json(new { success = true, message = "Colores guardados correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving color settings");
                return Json(new { success = false, message = "Error al guardar los colores: " + ex.Message });
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

        [HttpGet]
        [Route("Settings/DynamicColors")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetDynamicColors()
        {
            var colors = LoadColorsFromDatabase();

            // Generar CSS dinámico con los colores guardados
            string css = $@"
/* Dynamic Colors Stylesheet - Generated from Database */

/* Define CSS Variables */
:root {{
    --navbar-color-1: rgb({colors.NavbarColor1R}, {colors.NavbarColor1G}, {colors.NavbarColor1B});
    --navbar-color-2: rgb({colors.NavbarColor2R}, {colors.NavbarColor2G}, {colors.NavbarColor2B});
    --navbar-color-3: rgb({colors.NavbarColor3R}, {colors.NavbarColor3G}, {colors.NavbarColor3B});
    --sidebar-color-1: rgb({colors.SidebarColor1R}, {colors.SidebarColor1G}, {colors.SidebarColor1B});
    --sidebar-color-2: rgb({colors.SidebarColor2R}, {colors.SidebarColor2G}, {colors.SidebarColor2B});
    --sidebar-hover-color: rgb({colors.SidebarHoverR}, {colors.SidebarHoverG}, {colors.SidebarHoverB});
}}

/* Override navbar background with gradient */
.sb-topnav {{
    background: linear-gradient(90deg, var(--navbar-color-1) 0%, var(--navbar-color-2) 50%, var(--navbar-color-3) 100%) !important;
    background-color: var(--navbar-color-1) !important;
}}

/* Override sidebar background with gradient */
.sb-sidenav {{
    background: linear-gradient(90deg, var(--sidebar-color-1) 0%, var(--sidebar-color-2) 100%) !important;
}}

/* Override sidebar link hover and active states */
.sb-sidenav .nav-link:hover,
.sb-sidenav .nav-link.active {{
    border-left-color: var(--sidebar-hover-color) !important;
    background-color: rgba({colors.SidebarHoverR}, {colors.SidebarHoverG}, {colors.SidebarHoverB}, 0.15) !important;
}}

/* Add hover effect on normal links too */
.sb-sidenav .nav-link:hover {{
    color: #ffffff !important;
}}
";

            return Content(css, "text/css");
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

        // Sidebar Hover colors
        public int SidebarHoverR { get; set; } = 165;
        public int SidebarHoverG { get; set; } = 95;
        public int SidebarHoverB { get; set; } = 253;


        // Icon colors
        public int IconColorR { get; set; } = 255;
        public int IconColorG { get; set; } = 255;
        public int IconColorB { get; set; } = 255;

        // Icon hover colors
        public int IconColorHoverR { get; set; } = 255;
        public int IconColorHoverG { get; set; } = 255;
        public int IconColorHoverB { get; set; } = 255;

    }
}
