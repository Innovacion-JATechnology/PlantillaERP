using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PlantillaERP
{
    public partial class ColorSettings : Page
    {
        // Navbar colors (RGB)
        private const string NAVBAR_COLOR1_R = "NavbarColor1R";
        private const string NAVBAR_COLOR1_G = "NavbarColor1G";
        private const string NAVBAR_COLOR1_B = "NavbarColor1B";
        private const string NAVBAR_COLOR2_R = "NavbarColor2R";
        private const string NAVBAR_COLOR2_G = "NavbarColor2G";
        private const string NAVBAR_COLOR2_B = "NavbarColor2B";
        private const string NAVBAR_COLOR3_R = "NavbarColor3R";
        private const string NAVBAR_COLOR3_G = "NavbarColor3G";
        private const string NAVBAR_COLOR3_B = "NavbarColor3B";

        // Sidebar colors (RGB)
        private const string SIDEBAR_COLOR1_R = "SidebarColor1R";
        private const string SIDEBAR_COLOR1_G = "SidebarColor1G";
        private const string SIDEBAR_COLOR1_B = "SidebarColor1B";
        private const string SIDEBAR_COLOR2_R = "SidebarColor2R";
        private const string SIDEBAR_COLOR2_G = "SidebarColor2G";
        private const string SIDEBAR_COLOR2_B = "SidebarColor2B";

        // Sidebar Hover color (RGB)
        private const string SIDEBAR_HOVER_R = "SidebarHoverR";
        private const string SIDEBAR_HOVER_G = "SidebarHoverG";
        private const string SIDEBAR_HOVER_B = "SidebarHoverB";

        // Sidebar Hover Border color (RGB)
        private const string SIDEBAR_HOVER_BORDER_R = "SidebarHoverBorderR";
        private const string SIDEBAR_HOVER_BORDER_G = "SidebarHoverBorderG";
        private const string SIDEBAR_HOVER_BORDER_B = "SidebarHoverBorderB";

        // Module colors (RGB)
        private const string MODULE_COLOR1_R = "ModuleColor1R";
        private const string MODULE_COLOR1_G = "ModuleColor1G";
        private const string MODULE_COLOR1_B = "ModuleColor1B";
        private const string MODULE_COLOR2_R = "ModuleColor2R";
        private const string MODULE_COLOR2_G = "ModuleColor2G";
        private const string MODULE_COLOR2_B = "ModuleColor2B";
        private const string MODULE_COLOR3_R = "ModuleColor3R";
        private const string MODULE_COLOR3_G = "ModuleColor3G";
        private const string MODULE_COLOR3_B = "ModuleColor3B";
        private const string MODULE_ICON_R = "ModuleIconR";
        private const string MODULE_ICON_G = "ModuleIconG";
        private const string MODULE_ICON_B = "ModuleIconB";

        // Default values
        private class DefaultColors
        {
            public int Navbar1R { get; set; } = 87;
            public int Navbar1G { get; set; } = 179;
            public int Navbar1B { get; set; } = 252;
            public int Navbar2R { get; set; } = 165;
            public int Navbar2G { get; set; } = 95;
            public int Navbar2B { get; set; } = 253;
            public int Navbar3R { get; set; } = 16;
            public int Navbar3G { get; set; } = 55;
            public int Navbar3B { get; set; } = 161;
            public int Sidebar1R { get; set; } = 87;
            public int Sidebar1G { get; set; } = 179;
            public int Sidebar1B { get; set; } = 252;
            public int Sidebar2R { get; set; } = 130;
            public int Sidebar2G { get; set; } = 157;
            public int Sidebar2B { get; set; } = 245;
            public int SidebarHoverR { get; set; } = 255;
            public int SidebarHoverG { get; set; } = 255;
            public int SidebarHoverB { get; set; } = 255;
            public int SidebarHoverBorderR { get; set; } = 165;
            public int SidebarHoverBorderG { get; set; } = 95;
            public int SidebarHoverBorderB { get; set; } = 253;
            public int Module1R { get; set; } = 87;
            public int Module1G { get; set; } = 179;
            public int Module1B { get; set; } = 252;
            public int Module2R { get; set; } = 165;
            public int Module2G { get; set; } = 95;
            public int Module2B { get; set; } = 253;
            public int Module3R { get; set; } = 16;
            public int Module3G { get; set; } = 55;
            public int Module3B { get; set; } = 161;
            public int ModuleIconR { get; set; } = 87;
            public int ModuleIconG { get; set; } = 179;
            public int ModuleIconB { get; set; } = 252;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadColors();
            }
        }

        private void LoadColors()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ServerCon"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = ConfigurationManager.ConnectionStrings["LocalCon"]?.ConnectionString;
                }

                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ColorSettings')
                            BEGIN
                                CREATE TABLE ColorSettings (
                                    Id INT PRIMARY KEY IDENTITY(1,1),
                                    SettingKey NVARCHAR(50) UNIQUE NOT NULL,
                                    SettingValue NVARCHAR(50) NOT NULL,
                                    UpdatedDate DATETIME DEFAULT GETDATE()
                                )
                            END";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Load all color values
                        string[] allKeys = {
                            NAVBAR_COLOR1_R, NAVBAR_COLOR1_G, NAVBAR_COLOR1_B,
                            NAVBAR_COLOR2_R, NAVBAR_COLOR2_G, NAVBAR_COLOR2_B,
                            NAVBAR_COLOR3_R, NAVBAR_COLOR3_G, NAVBAR_COLOR3_B,
                            SIDEBAR_COLOR1_R, SIDEBAR_COLOR1_G, SIDEBAR_COLOR1_B,
                            SIDEBAR_COLOR2_R, SIDEBAR_COLOR2_G, SIDEBAR_COLOR2_B,
                            SIDEBAR_HOVER_R, SIDEBAR_HOVER_G, SIDEBAR_HOVER_B,
                            SIDEBAR_HOVER_BORDER_R, SIDEBAR_HOVER_BORDER_G, SIDEBAR_HOVER_BORDER_B,
                            MODULE_COLOR1_R, MODULE_COLOR1_G, MODULE_COLOR1_B,
                            MODULE_COLOR2_R, MODULE_COLOR2_G, MODULE_COLOR2_B,
                            MODULE_COLOR3_R, MODULE_COLOR3_G, MODULE_COLOR3_B,
                            MODULE_ICON_R, MODULE_ICON_G, MODULE_ICON_B
                        };

                        query = "SELECT SettingKey, SettingValue FROM ColorSettings WHERE SettingKey IN ('" + 
                                string.Join("','", allKeys) + "')";
                        
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string key = reader["SettingKey"].ToString();
                                    string value = reader["SettingValue"].ToString();
                                    AssignColorValue(key, value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading colors: {ex.Message}");
            }

            SetDefaultValues();
        }

        private void AssignColorValue(string key, string value)
        {
            switch (key)
            {
                case NAVBAR_COLOR1_R: txtNavbar1R.Text = value; break;
                case NAVBAR_COLOR1_G: txtNavbar1G.Text = value; break;
                case NAVBAR_COLOR1_B: txtNavbar1B.Text = value; break;
                case NAVBAR_COLOR2_R: txtNavbar2R.Text = value; break;
                case NAVBAR_COLOR2_G: txtNavbar2G.Text = value; break;
                case NAVBAR_COLOR2_B: txtNavbar2B.Text = value; break;
                case NAVBAR_COLOR3_R: txtNavbar3R.Text = value; break;
                case NAVBAR_COLOR3_G: txtNavbar3G.Text = value; break;
                case NAVBAR_COLOR3_B: txtNavbar3B.Text = value; break;
                case SIDEBAR_COLOR1_R: txtSidebar1R.Text = value; break;
                case SIDEBAR_COLOR1_G: txtSidebar1G.Text = value; break;
                case SIDEBAR_COLOR1_B: txtSidebar1B.Text = value; break;
                case SIDEBAR_COLOR2_R: txtSidebar2R.Text = value; break;
                case SIDEBAR_COLOR2_G: txtSidebar2G.Text = value; break;
                case SIDEBAR_COLOR2_B: txtSidebar2B.Text = value; break;
                case SIDEBAR_HOVER_R: txtSidebarHoverR.Text = value; break;
                case SIDEBAR_HOVER_G: txtSidebarHoverG.Text = value; break;
                case SIDEBAR_HOVER_B: txtSidebarHoverB.Text = value; break;
                case SIDEBAR_HOVER_BORDER_R: txtSidebarHoverBorderR.Text = value; break;
                case SIDEBAR_HOVER_BORDER_G: txtSidebarHoverBorderG.Text = value; break;
                case SIDEBAR_HOVER_BORDER_B: txtSidebarHoverBorderB.Text = value; break;
                case MODULE_COLOR1_R: txtModule1R.Text = value; break;
                case MODULE_COLOR1_G: txtModule1G.Text = value; break;
                case MODULE_COLOR1_B: txtModule1B.Text = value; break;
                case MODULE_COLOR2_R: txtModule2R.Text = value; break;
                case MODULE_COLOR2_G: txtModule2G.Text = value; break;
                case MODULE_COLOR2_B: txtModule2B.Text = value; break;
                case MODULE_COLOR3_R: txtModule3R.Text = value; break;
                case MODULE_COLOR3_G: txtModule3G.Text = value; break;
                case MODULE_COLOR3_B: txtModule3B.Text = value; break;
            }
        }

        private void SetDefaultValues()
        {
            var defaults = new DefaultColors();

            if (string.IsNullOrEmpty(txtNavbar1R.Text)) txtNavbar1R.Text = defaults.Navbar1R.ToString();
            if (string.IsNullOrEmpty(txtNavbar1G.Text)) txtNavbar1G.Text = defaults.Navbar1G.ToString();
            if (string.IsNullOrEmpty(txtNavbar1B.Text)) txtNavbar1B.Text = defaults.Navbar1B.ToString();
            if (string.IsNullOrEmpty(txtNavbar2R.Text)) txtNavbar2R.Text = defaults.Navbar2R.ToString();
            if (string.IsNullOrEmpty(txtNavbar2G.Text)) txtNavbar2G.Text = defaults.Navbar2G.ToString();
            if (string.IsNullOrEmpty(txtNavbar2B.Text)) txtNavbar2B.Text = defaults.Navbar2B.ToString();
            if (string.IsNullOrEmpty(txtNavbar3R.Text)) txtNavbar3R.Text = defaults.Navbar3R.ToString();
            if (string.IsNullOrEmpty(txtNavbar3G.Text)) txtNavbar3G.Text = defaults.Navbar3G.ToString();
            if (string.IsNullOrEmpty(txtNavbar3B.Text)) txtNavbar3B.Text = defaults.Navbar3B.ToString();
            if (string.IsNullOrEmpty(txtSidebar1R.Text)) txtSidebar1R.Text = defaults.Sidebar1R.ToString();
            if (string.IsNullOrEmpty(txtSidebar1G.Text)) txtSidebar1G.Text = defaults.Sidebar1G.ToString();
            if (string.IsNullOrEmpty(txtSidebar1B.Text)) txtSidebar1B.Text = defaults.Sidebar1B.ToString();
            if (string.IsNullOrEmpty(txtSidebar2R.Text)) txtSidebar2R.Text = defaults.Sidebar2R.ToString();
            if (string.IsNullOrEmpty(txtSidebar2G.Text)) txtSidebar2G.Text = defaults.Sidebar2G.ToString();
            if (string.IsNullOrEmpty(txtSidebar2B.Text)) txtSidebar2B.Text = defaults.Sidebar2B.ToString();
            if (string.IsNullOrEmpty(txtSidebarHoverR.Text)) txtSidebarHoverR.Text = defaults.SidebarHoverR.ToString();
            if (string.IsNullOrEmpty(txtSidebarHoverG.Text)) txtSidebarHoverG.Text = defaults.SidebarHoverG.ToString();
            if (string.IsNullOrEmpty(txtSidebarHoverB.Text)) txtSidebarHoverB.Text = defaults.SidebarHoverB.ToString();
            if (string.IsNullOrEmpty(txtSidebarHoverBorderR.Text)) txtSidebarHoverBorderR.Text = defaults.SidebarHoverBorderR.ToString();
            if (string.IsNullOrEmpty(txtSidebarHoverBorderG.Text)) txtSidebarHoverBorderG.Text = defaults.SidebarHoverBorderG.ToString();
            if (string.IsNullOrEmpty(txtSidebarHoverBorderB.Text)) txtSidebarHoverBorderB.Text = defaults.SidebarHoverBorderB.ToString();
            if (string.IsNullOrEmpty(txtModule1R.Text)) txtModule1R.Text = defaults.Module1R.ToString();
            if (string.IsNullOrEmpty(txtModule1G.Text)) txtModule1G.Text = defaults.Module1G.ToString();
            if (string.IsNullOrEmpty(txtModule1B.Text)) txtModule1B.Text = defaults.Module1B.ToString();
            if (string.IsNullOrEmpty(txtModule2R.Text)) txtModule2R.Text = defaults.Module2R.ToString();
            if (string.IsNullOrEmpty(txtModule2G.Text)) txtModule2G.Text = defaults.Module2G.ToString();
            if (string.IsNullOrEmpty(txtModule2B.Text)) txtModule2B.Text = defaults.Module2B.ToString();
            if (string.IsNullOrEmpty(txtModule3R.Text)) txtModule3R.Text = defaults.Module3R.ToString();
            if (string.IsNullOrEmpty(txtModule3G.Text)) txtModule3G.Text = defaults.Module3G.ToString();
            if (string.IsNullOrEmpty(txtModule3B.Text)) txtModule3B.Text = defaults.Module3B.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Determine which tab is active and call the appropriate save method
            Button btnClicked = sender as Button;

            if (btnClicked != null)
            {
                switch (btnClicked.ID)
                {
                    case "btnSaveNavbar":
                        SaveNavbarColors();
                        break;
                    case "btnSidebarSave":
                        SaveSidebarColors();
                        break;
                    case "btnModulesSave":
                        SaveModuleColors();
                        break;
                    case "btnSidebarHoverSave":
                        SaveSidebarHoverColors();
                        break;
                    default:
                        SaveAllColors();
                        break;
                }
            }
            else
            {
                SaveAllColors();
            }
        }

        private Dictionary<string, int> LoadColorsFromDatabase()
        {
            var colorDict = new Dictionary<string, int>();
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ServerCon"]?.ConnectionString;
                if (string.IsNullOrEmpty(connString))
                {
                    connString = ConfigurationManager.ConnectionStrings["LocalCon"]?.ConnectionString;
                }

                if (!string.IsNullOrEmpty(connString))
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string query = "SELECT SettingKey, SettingValue FROM ColorSettings";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string key = reader["SettingKey"].ToString();
                                    if (int.TryParse(reader["SettingValue"].ToString(), out int value))
                                    {
                                        colorDict[key] = value;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading colors from database: {ex.Message}");
            }
            return colorDict;
        }

        private void SaveNavbarColors()
        {
            try
            {
                // Validate Navbar colors only
                int navbar1R = ValidateRGB(txtNavbar1R.Text.Trim(), "Navbar Color 1 - Red");
                int navbar1G = ValidateRGB(txtNavbar1G.Text.Trim(), "Navbar Color 1 - Green");
                int navbar1B = ValidateRGB(txtNavbar1B.Text.Trim(), "Navbar Color 1 - Blue");
                int navbar2R = ValidateRGB(txtNavbar2R.Text.Trim(), "Navbar Color 2 - Red");
                int navbar2G = ValidateRGB(txtNavbar2G.Text.Trim(), "Navbar Color 2 - Green");
                int navbar2B = ValidateRGB(txtNavbar2B.Text.Trim(), "Navbar Color 2 - Blue");
                int navbar3R = ValidateRGB(txtNavbar3R.Text.Trim(), "Navbar Color 3 - Red");
                int navbar3G = ValidateRGB(txtNavbar3G.Text.Trim(), "Navbar Color 3 - Green");
                int navbar3B = ValidateRGB(txtNavbar3B.Text.Trim(), "Navbar Color 3 - Blue");

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int sidebar1R = colorDict.ContainsKey(SIDEBAR_COLOR1_R) ? colorDict[SIDEBAR_COLOR1_R] : 87;
                int sidebar1G = colorDict.ContainsKey(SIDEBAR_COLOR1_G) ? colorDict[SIDEBAR_COLOR1_G] : 179;
                int sidebar1B = colorDict.ContainsKey(SIDEBAR_COLOR1_B) ? colorDict[SIDEBAR_COLOR1_B] : 252;
                int sidebar2R = colorDict.ContainsKey(SIDEBAR_COLOR2_R) ? colorDict[SIDEBAR_COLOR2_R] : 130;
                int sidebar2G = colorDict.ContainsKey(SIDEBAR_COLOR2_G) ? colorDict[SIDEBAR_COLOR2_G] : 157;
                int sidebar2B = colorDict.ContainsKey(SIDEBAR_COLOR2_B) ? colorDict[SIDEBAR_COLOR2_B] : 245;
                int sidebarHoverR = colorDict.ContainsKey(SIDEBAR_HOVER_R) ? colorDict[SIDEBAR_HOVER_R] : 255;
                int sidebarHoverG = colorDict.ContainsKey(SIDEBAR_HOVER_G) ? colorDict[SIDEBAR_HOVER_G] : 255;
                int sidebarHoverB = colorDict.ContainsKey(SIDEBAR_HOVER_B) ? colorDict[SIDEBAR_HOVER_B] : 255;
                int sidebarHoverBorderR = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_R) ? colorDict[SIDEBAR_HOVER_BORDER_R] : 165;
                int sidebarHoverBorderG = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_G) ? colorDict[SIDEBAR_HOVER_BORDER_G] : 95;
                int sidebarHoverBorderB = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_B) ? colorDict[SIDEBAR_HOVER_BORDER_B] : 253;
                int module1R = colorDict.ContainsKey(MODULE_COLOR1_R) ? colorDict[MODULE_COLOR1_R] : 87;
                int module1G = colorDict.ContainsKey(MODULE_COLOR1_G) ? colorDict[MODULE_COLOR1_G] : 179;
                int module1B = colorDict.ContainsKey(MODULE_COLOR1_B) ? colorDict[MODULE_COLOR1_B] : 252;
                int module2R = colorDict.ContainsKey(MODULE_COLOR2_R) ? colorDict[MODULE_COLOR2_R] : 165;
                int module2G = colorDict.ContainsKey(MODULE_COLOR2_G) ? colorDict[MODULE_COLOR2_G] : 95;
                int module2B = colorDict.ContainsKey(MODULE_COLOR2_B) ? colorDict[MODULE_COLOR2_B] : 253;
                int module3R = colorDict.ContainsKey(MODULE_COLOR3_R) ? colorDict[MODULE_COLOR3_R] : 16;
                int module3G = colorDict.ContainsKey(MODULE_COLOR3_G) ? colorDict[MODULE_COLOR3_G] : 55;
                int module3B = colorDict.ContainsKey(MODULE_COLOR3_B) ? colorDict[MODULE_COLOR3_B] : 161;

                // Save only Navbar colors to database
                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                // Generate custom CSS files
                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores de la Barra de Navegacion guardados exitosamente! Recarga la pagina para ver los cambios.", "alert-success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al guardar los colores: {ex.Message}", "alert-danger");
            }
        }

        private void SaveSidebarColors()
        {
            try
            {
                // Validate Sidebar colors only
                int sidebar1R = ValidateRGB(txtSidebar1R.Text.Trim(), "Sidebar Color 1 - Red");
                int sidebar1G = ValidateRGB(txtSidebar1G.Text.Trim(), "Sidebar Color 1 - Green");
                int sidebar1B = ValidateRGB(txtSidebar1B.Text.Trim(), "Sidebar Color 1 - Blue");
                int sidebar2R = ValidateRGB(txtSidebar2R.Text.Trim(), "Sidebar Color 2 - Red");
                int sidebar2G = ValidateRGB(txtSidebar2G.Text.Trim(), "Sidebar Color 2 - Green");
                int sidebar2B = ValidateRGB(txtSidebar2B.Text.Trim(), "Sidebar Color 2 - Blue");

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int navbar1R = colorDict.ContainsKey(NAVBAR_COLOR1_R) ? colorDict[NAVBAR_COLOR1_R] : 87;
                int navbar1G = colorDict.ContainsKey(NAVBAR_COLOR1_G) ? colorDict[NAVBAR_COLOR1_G] : 179;
                int navbar1B = colorDict.ContainsKey(NAVBAR_COLOR1_B) ? colorDict[NAVBAR_COLOR1_B] : 252;
                int navbar2R = colorDict.ContainsKey(NAVBAR_COLOR2_R) ? colorDict[NAVBAR_COLOR2_R] : 165;
                int navbar2G = colorDict.ContainsKey(NAVBAR_COLOR2_G) ? colorDict[NAVBAR_COLOR2_G] : 95;
                int navbar2B = colorDict.ContainsKey(NAVBAR_COLOR2_B) ? colorDict[NAVBAR_COLOR2_B] : 253;
                int navbar3R = colorDict.ContainsKey(NAVBAR_COLOR3_R) ? colorDict[NAVBAR_COLOR3_R] : 16;
                int navbar3G = colorDict.ContainsKey(NAVBAR_COLOR3_G) ? colorDict[NAVBAR_COLOR3_G] : 55;
                int navbar3B = colorDict.ContainsKey(NAVBAR_COLOR3_B) ? colorDict[NAVBAR_COLOR3_B] : 161;
                int sidebarHoverR = colorDict.ContainsKey(SIDEBAR_HOVER_R) ? colorDict[SIDEBAR_HOVER_R] : 255;
                int sidebarHoverG = colorDict.ContainsKey(SIDEBAR_HOVER_G) ? colorDict[SIDEBAR_HOVER_G] : 255;
                int sidebarHoverB = colorDict.ContainsKey(SIDEBAR_HOVER_B) ? colorDict[SIDEBAR_HOVER_B] : 255;
                int sidebarHoverBorderR = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_R) ? colorDict[SIDEBAR_HOVER_BORDER_R] : 165;
                int sidebarHoverBorderG = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_G) ? colorDict[SIDEBAR_HOVER_BORDER_G] : 95;
                int sidebarHoverBorderB = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_B) ? colorDict[SIDEBAR_HOVER_BORDER_B] : 253;
                int module1R = colorDict.ContainsKey(MODULE_COLOR1_R) ? colorDict[MODULE_COLOR1_R] : 87;
                int module1G = colorDict.ContainsKey(MODULE_COLOR1_G) ? colorDict[MODULE_COLOR1_G] : 179;
                int module1B = colorDict.ContainsKey(MODULE_COLOR1_B) ? colorDict[MODULE_COLOR1_B] : 252;
                int module2R = colorDict.ContainsKey(MODULE_COLOR2_R) ? colorDict[MODULE_COLOR2_R] : 165;
                int module2G = colorDict.ContainsKey(MODULE_COLOR2_G) ? colorDict[MODULE_COLOR2_G] : 95;
                int module2B = colorDict.ContainsKey(MODULE_COLOR2_B) ? colorDict[MODULE_COLOR2_B] : 253;
                int module3R = colorDict.ContainsKey(MODULE_COLOR3_R) ? colorDict[MODULE_COLOR3_R] : 16;
                int module3G = colorDict.ContainsKey(MODULE_COLOR3_G) ? colorDict[MODULE_COLOR3_G] : 55;
                int module3B = colorDict.ContainsKey(MODULE_COLOR3_B) ? colorDict[MODULE_COLOR3_B] : 161;

                // Save all colors but update only Sidebar values
                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores de la Barra Lateral guardados exitosamente! Recarga la pagina para ver los cambios.", "alert-success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al guardar los colores: {ex.Message}", "alert-danger");
            }
        }

        private void SaveModuleColors()
        {
            try
            {
                // Validate Module colors only
                int module1R = ValidateRGB(txtModule1R.Text.Trim(), "Module Color 1 - Red");
                int module1G = ValidateRGB(txtModule1G.Text.Trim(), "Module Color 1 - Green");
                int module1B = ValidateRGB(txtModule1B.Text.Trim(), "Module Color 1 - Blue");
                int module2R = ValidateRGB(txtModule2R.Text.Trim(), "Module Color 2 - Red");
                int module2G = ValidateRGB(txtModule2G.Text.Trim(), "Module Color 2 - Green");
                int module2B = ValidateRGB(txtModule2B.Text.Trim(), "Module Color 2 - Blue");
                int module3R = ValidateRGB(txtModule3R.Text.Trim(), "Module Color 3 - Red");
                int module3G = ValidateRGB(txtModule3G.Text.Trim(), "Module Color 3 - Green");
                int module3B = ValidateRGB(txtModule3B.Text.Trim(), "Module Color 3 - Blue");

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int navbar1R = colorDict.ContainsKey(NAVBAR_COLOR1_R) ? colorDict[NAVBAR_COLOR1_R] : 87;
                int navbar1G = colorDict.ContainsKey(NAVBAR_COLOR1_G) ? colorDict[NAVBAR_COLOR1_G] : 179;
                int navbar1B = colorDict.ContainsKey(NAVBAR_COLOR1_B) ? colorDict[NAVBAR_COLOR1_B] : 252;
                int navbar2R = colorDict.ContainsKey(NAVBAR_COLOR2_R) ? colorDict[NAVBAR_COLOR2_R] : 165;
                int navbar2G = colorDict.ContainsKey(NAVBAR_COLOR2_G) ? colorDict[NAVBAR_COLOR2_G] : 95;
                int navbar2B = colorDict.ContainsKey(NAVBAR_COLOR2_B) ? colorDict[NAVBAR_COLOR2_B] : 253;
                int navbar3R = colorDict.ContainsKey(NAVBAR_COLOR3_R) ? colorDict[NAVBAR_COLOR3_R] : 16;
                int navbar3G = colorDict.ContainsKey(NAVBAR_COLOR3_G) ? colorDict[NAVBAR_COLOR3_G] : 55;
                int navbar3B = colorDict.ContainsKey(NAVBAR_COLOR3_B) ? colorDict[NAVBAR_COLOR3_B] : 161;
                int sidebar1R = colorDict.ContainsKey(SIDEBAR_COLOR1_R) ? colorDict[SIDEBAR_COLOR1_R] : 87;
                int sidebar1G = colorDict.ContainsKey(SIDEBAR_COLOR1_G) ? colorDict[SIDEBAR_COLOR1_G] : 179;
                int sidebar1B = colorDict.ContainsKey(SIDEBAR_COLOR1_B) ? colorDict[SIDEBAR_COLOR1_B] : 252;
                int sidebar2R = colorDict.ContainsKey(SIDEBAR_COLOR2_R) ? colorDict[SIDEBAR_COLOR2_R] : 130;
                int sidebar2G = colorDict.ContainsKey(SIDEBAR_COLOR2_G) ? colorDict[SIDEBAR_COLOR2_G] : 157;
                int sidebar2B = colorDict.ContainsKey(SIDEBAR_COLOR2_B) ? colorDict[SIDEBAR_COLOR2_B] : 245;
                int sidebarHoverR = colorDict.ContainsKey(SIDEBAR_HOVER_R) ? colorDict[SIDEBAR_HOVER_R] : 255;
                int sidebarHoverG = colorDict.ContainsKey(SIDEBAR_HOVER_G) ? colorDict[SIDEBAR_HOVER_G] : 255;
                int sidebarHoverB = colorDict.ContainsKey(SIDEBAR_HOVER_B) ? colorDict[SIDEBAR_HOVER_B] : 255;
                int sidebarHoverBorderR = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_R) ? colorDict[SIDEBAR_HOVER_BORDER_R] : 165;
                int sidebarHoverBorderG = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_G) ? colorDict[SIDEBAR_HOVER_BORDER_G] : 95;
                int sidebarHoverBorderB = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_B) ? colorDict[SIDEBAR_HOVER_BORDER_B] : 253;

                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores de los Modulos guardados exitosamente! Recarga la pagina para ver los cambios.", "alert-success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al guardar los colores: {ex.Message}", "alert-danger");
            }
        }

        private void SaveSidebarHoverColors()
        {
            try
            {
                // Validate Sidebar Hover colors only
                int sidebarHoverR = ValidateRGB(txtSidebarHoverR.Text.Trim(), "Sidebar Hover Color - Red");
                int sidebarHoverG = ValidateRGB(txtSidebarHoverG.Text.Trim(), "Sidebar Hover Color - Green");
                int sidebarHoverB = ValidateRGB(txtSidebarHoverB.Text.Trim(), "Sidebar Hover Color - Blue");
                int sidebarHoverBorderR = ValidateRGB(txtSidebarHoverBorderR.Text.Trim(), "Sidebar Hover Border Color - Red");
                int sidebarHoverBorderG = ValidateRGB(txtSidebarHoverBorderG.Text.Trim(), "Sidebar Hover Border Color - Green");
                int sidebarHoverBorderB = ValidateRGB(txtSidebarHoverBorderB.Text.Trim(), "Sidebar Hover Border Color - Blue");

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int navbar1R = colorDict.ContainsKey(NAVBAR_COLOR1_R) ? colorDict[NAVBAR_COLOR1_R] : 87;
                int navbar1G = colorDict.ContainsKey(NAVBAR_COLOR1_G) ? colorDict[NAVBAR_COLOR1_G] : 179;
                int navbar1B = colorDict.ContainsKey(NAVBAR_COLOR1_B) ? colorDict[NAVBAR_COLOR1_B] : 252;
                int navbar2R = colorDict.ContainsKey(NAVBAR_COLOR2_R) ? colorDict[NAVBAR_COLOR2_R] : 165;
                int navbar2G = colorDict.ContainsKey(NAVBAR_COLOR2_G) ? colorDict[NAVBAR_COLOR2_G] : 95;
                int navbar2B = colorDict.ContainsKey(NAVBAR_COLOR2_B) ? colorDict[NAVBAR_COLOR2_B] : 253;
                int navbar3R = colorDict.ContainsKey(NAVBAR_COLOR3_R) ? colorDict[NAVBAR_COLOR3_R] : 16;
                int navbar3G = colorDict.ContainsKey(NAVBAR_COLOR3_G) ? colorDict[NAVBAR_COLOR3_G] : 55;
                int navbar3B = colorDict.ContainsKey(NAVBAR_COLOR3_B) ? colorDict[NAVBAR_COLOR3_B] : 161;
                int sidebar1R = colorDict.ContainsKey(SIDEBAR_COLOR1_R) ? colorDict[SIDEBAR_COLOR1_R] : 87;
                int sidebar1G = colorDict.ContainsKey(SIDEBAR_COLOR1_G) ? colorDict[SIDEBAR_COLOR1_G] : 179;
                int sidebar1B = colorDict.ContainsKey(SIDEBAR_COLOR1_B) ? colorDict[SIDEBAR_COLOR1_B] : 252;
                int sidebar2R = colorDict.ContainsKey(SIDEBAR_COLOR2_R) ? colorDict[SIDEBAR_COLOR2_R] : 130;
                int sidebar2G = colorDict.ContainsKey(SIDEBAR_COLOR2_G) ? colorDict[SIDEBAR_COLOR2_G] : 157;
                int sidebar2B = colorDict.ContainsKey(SIDEBAR_COLOR2_B) ? colorDict[SIDEBAR_COLOR2_B] : 245;
                int module1R = colorDict.ContainsKey(MODULE_COLOR1_R) ? colorDict[MODULE_COLOR1_R] : 87;
                int module1G = colorDict.ContainsKey(MODULE_COLOR1_G) ? colorDict[MODULE_COLOR1_G] : 179;
                int module1B = colorDict.ContainsKey(MODULE_COLOR1_B) ? colorDict[MODULE_COLOR1_B] : 252;
                int module2R = colorDict.ContainsKey(MODULE_COLOR2_R) ? colorDict[MODULE_COLOR2_R] : 165;
                int module2G = colorDict.ContainsKey(MODULE_COLOR2_G) ? colorDict[MODULE_COLOR2_G] : 95;
                int module2B = colorDict.ContainsKey(MODULE_COLOR2_B) ? colorDict[MODULE_COLOR2_B] : 253;
                int module3R = colorDict.ContainsKey(MODULE_COLOR3_R) ? colorDict[MODULE_COLOR3_R] : 16;
                int module3G = colorDict.ContainsKey(MODULE_COLOR3_G) ? colorDict[MODULE_COLOR3_G] : 55;
                int module3B = colorDict.ContainsKey(MODULE_COLOR3_B) ? colorDict[MODULE_COLOR3_B] : 161;

                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores del Hover del Sidebar guardados exitosamente! Recarga la pagina para ver los cambios.", "alert-success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al guardar los colores: {ex.Message}", "alert-danger");
            }
        }

        private void SaveAllColors()
        {
            try
            {
                // Validate all colors - Navbar
                int navbar1R = ValidateRGB(txtNavbar1R.Text.Trim(), "Navbar Color 1 - Red");
                int navbar1G = ValidateRGB(txtNavbar1G.Text.Trim(), "Navbar Color 1 - Green");
                int navbar1B = ValidateRGB(txtNavbar1B.Text.Trim(), "Navbar Color 1 - Blue");
                int navbar2R = ValidateRGB(txtNavbar2R.Text.Trim(), "Navbar Color 2 - Red");
                int navbar2G = ValidateRGB(txtNavbar2G.Text.Trim(), "Navbar Color 2 - Green");
                int navbar2B = ValidateRGB(txtNavbar2B.Text.Trim(), "Navbar Color 2 - Blue");
                int navbar3R = ValidateRGB(txtNavbar3R.Text.Trim(), "Navbar Color 3 - Red");
                int navbar3G = ValidateRGB(txtNavbar3G.Text.Trim(), "Navbar Color 3 - Green");
                int navbar3B = ValidateRGB(txtNavbar3B.Text.Trim(), "Navbar Color 3 - Blue");

                // Validate Sidebar
                int sidebar1R = ValidateRGB(txtSidebar1R.Text.Trim(), "Sidebar Color 1 - Red");
                int sidebar1G = ValidateRGB(txtSidebar1G.Text.Trim(), "Sidebar Color 1 - Green");
                int sidebar1B = ValidateRGB(txtSidebar1B.Text.Trim(), "Sidebar Color 1 - Blue");
                int sidebar2R = ValidateRGB(txtSidebar2R.Text.Trim(), "Sidebar Color 2 - Red");
                int sidebar2G = ValidateRGB(txtSidebar2G.Text.Trim(), "Sidebar Color 2 - Green");
                int sidebar2B = ValidateRGB(txtSidebar2B.Text.Trim(), "Sidebar Color 2 - Blue");

                // Validate Sidebar Hover
                int sidebarHoverR = ValidateRGB(txtSidebarHoverR.Text.Trim(), "Sidebar Hover Color - Red");
                int sidebarHoverG = ValidateRGB(txtSidebarHoverG.Text.Trim(), "Sidebar Hover Color - Green");
                int sidebarHoverB = ValidateRGB(txtSidebarHoverB.Text.Trim(), "Sidebar Hover Color - Blue");

                // Validate Sidebar Hover Border
                int sidebarHoverBorderR = ValidateRGB(txtSidebarHoverBorderR.Text.Trim(), "Sidebar Hover Border Color - Red");
                int sidebarHoverBorderG = ValidateRGB(txtSidebarHoverBorderG.Text.Trim(), "Sidebar Hover Border Color - Green");
                int sidebarHoverBorderB = ValidateRGB(txtSidebarHoverBorderB.Text.Trim(), "Sidebar Hover Border Color - Blue");

                // Validate Module colors
                int module1R = ValidateRGB(txtModule1R.Text.Trim(), "Module Color 1 - Red");
                int module1G = ValidateRGB(txtModule1G.Text.Trim(), "Module Color 1 - Green");
                int module1B = ValidateRGB(txtModule1B.Text.Trim(), "Module Color 1 - Blue");
                int module2R = ValidateRGB(txtModule2R.Text.Trim(), "Module Color 2 - Red");
                int module2G = ValidateRGB(txtModule2G.Text.Trim(), "Module Color 2 - Green");
                int module2B = ValidateRGB(txtModule2B.Text.Trim(), "Module Color 2 - Blue");
                int module3R = ValidateRGB(txtModule3R.Text.Trim(), "Module Color 3 - Red");
                int module3G = ValidateRGB(txtModule3G.Text.Trim(), "Module Color 3 - Green");
                int module3B = ValidateRGB(txtModule3B.Text.Trim(), "Module Color 3 - Blue");

                // Save to database
                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                // Generate custom CSS files
                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores guardados exitosamente! Recarga la pagina para ver los cambios.", "alert-success");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al guardar los colores: {ex.Message}", "alert-danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // Determine which tab is active and call the appropriate reset method
            Button btnClicked = sender as Button;

            if (btnClicked != null)
            {
                switch (btnClicked.ID)
                {
                    case "btnResetNavbar":
                        ResetNavbarColors();
                        break;
                    case "btnSidebarReset":
                        ResetSidebarColors();
                        break;
                    case "btnModulesReset":
                        ResetModuleColors();
                        break;
                    case "btnSidebarHoverReset":
                        ResetSidebarHoverColors();
                        break;
                    default:
                        ResetAllColors();
                        break;
                }
            }
            else
            {
                ResetAllColors();
            }
        }

        private void ResetNavbarColors()
        {
            try
            {
                var defaults = new DefaultColors();

                // Reset only Navbar colors
                txtNavbar1R.Text = defaults.Navbar1R.ToString();
                txtNavbar1G.Text = defaults.Navbar1G.ToString();
                txtNavbar1B.Text = defaults.Navbar1B.ToString();
                txtNavbar2R.Text = defaults.Navbar2R.ToString();
                txtNavbar2G.Text = defaults.Navbar2G.ToString();
                txtNavbar2B.Text = defaults.Navbar2B.ToString();
                txtNavbar3R.Text = defaults.Navbar3R.ToString();
                txtNavbar3G.Text = defaults.Navbar3G.ToString();
                txtNavbar3B.Text = defaults.Navbar3B.ToString();

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int sidebar1R = colorDict.ContainsKey(SIDEBAR_COLOR1_R) ? colorDict[SIDEBAR_COLOR1_R] : 87;
                int sidebar1G = colorDict.ContainsKey(SIDEBAR_COLOR1_G) ? colorDict[SIDEBAR_COLOR1_G] : 179;
                int sidebar1B = colorDict.ContainsKey(SIDEBAR_COLOR1_B) ? colorDict[SIDEBAR_COLOR1_B] : 252;
                int sidebar2R = colorDict.ContainsKey(SIDEBAR_COLOR2_R) ? colorDict[SIDEBAR_COLOR2_R] : 130;
                int sidebar2G = colorDict.ContainsKey(SIDEBAR_COLOR2_G) ? colorDict[SIDEBAR_COLOR2_G] : 157;
                int sidebar2B = colorDict.ContainsKey(SIDEBAR_COLOR2_B) ? colorDict[SIDEBAR_COLOR2_B] : 245;
                int sidebarHoverR = colorDict.ContainsKey(SIDEBAR_HOVER_R) ? colorDict[SIDEBAR_HOVER_R] : 255;
                int sidebarHoverG = colorDict.ContainsKey(SIDEBAR_HOVER_G) ? colorDict[SIDEBAR_HOVER_G] : 255;
                int sidebarHoverB = colorDict.ContainsKey(SIDEBAR_HOVER_B) ? colorDict[SIDEBAR_HOVER_B] : 255;
                int sidebarHoverBorderR = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_R) ? colorDict[SIDEBAR_HOVER_BORDER_R] : 165;
                int sidebarHoverBorderG = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_G) ? colorDict[SIDEBAR_HOVER_BORDER_G] : 95;
                int sidebarHoverBorderB = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_B) ? colorDict[SIDEBAR_HOVER_BORDER_B] : 253;
                int module1R = colorDict.ContainsKey(MODULE_COLOR1_R) ? colorDict[MODULE_COLOR1_R] : 87;
                int module1G = colorDict.ContainsKey(MODULE_COLOR1_G) ? colorDict[MODULE_COLOR1_G] : 179;
                int module1B = colorDict.ContainsKey(MODULE_COLOR1_B) ? colorDict[MODULE_COLOR1_B] : 252;
                int module2R = colorDict.ContainsKey(MODULE_COLOR2_R) ? colorDict[MODULE_COLOR2_R] : 165;
                int module2G = colorDict.ContainsKey(MODULE_COLOR2_G) ? colorDict[MODULE_COLOR2_G] : 95;
                int module2B = colorDict.ContainsKey(MODULE_COLOR2_B) ? colorDict[MODULE_COLOR2_B] : 253;
                int module3R = colorDict.ContainsKey(MODULE_COLOR3_R) ? colorDict[MODULE_COLOR3_R] : 16;
                int module3G = colorDict.ContainsKey(MODULE_COLOR3_G) ? colorDict[MODULE_COLOR3_G] : 55;
                int module3B = colorDict.ContainsKey(MODULE_COLOR3_B) ? colorDict[MODULE_COLOR3_B] : 161;

                SaveColorsToDatabase(
                    defaults.Navbar1R, defaults.Navbar1G, defaults.Navbar1B,
                    defaults.Navbar2R, defaults.Navbar2G, defaults.Navbar2B,
                    defaults.Navbar3R, defaults.Navbar3G, defaults.Navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    defaults.ModuleIconR, defaults.ModuleIconG, defaults.ModuleIconB
                );

                GenerateCustomCSS(
                    defaults.Navbar1R, defaults.Navbar1G, defaults.Navbar1B,
                    defaults.Navbar2R, defaults.Navbar2G, defaults.Navbar2B,
                    defaults.Navbar3R, defaults.Navbar3G, defaults.Navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    defaults.ModuleIconR, defaults.ModuleIconG, defaults.ModuleIconB
                );

                ShowMessage("Colores de la Barra de Navegacion restablecidos a los valores predeterminados! Recarga la pagina para ver los cambios.", "alert-info");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al restablecer los colores: {ex.Message}", "alert-danger");
            }
        }

        private void ResetSidebarColors()
        {
            try
            {
                var defaults = new DefaultColors();

                // Reset only Sidebar colors
                txtSidebar1R.Text = defaults.Sidebar1R.ToString();
                txtSidebar1G.Text = defaults.Sidebar1G.ToString();
                txtSidebar1B.Text = defaults.Sidebar1B.ToString();
                txtSidebar2R.Text = defaults.Sidebar2R.ToString();
                txtSidebar2G.Text = defaults.Sidebar2G.ToString();
                txtSidebar2B.Text = defaults.Sidebar2B.ToString();

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int navbar1R = colorDict.ContainsKey(NAVBAR_COLOR1_R) ? colorDict[NAVBAR_COLOR1_R] : 87;
                int navbar1G = colorDict.ContainsKey(NAVBAR_COLOR1_G) ? colorDict[NAVBAR_COLOR1_G] : 179;
                int navbar1B = colorDict.ContainsKey(NAVBAR_COLOR1_B) ? colorDict[NAVBAR_COLOR1_B] : 252;
                int navbar2R = colorDict.ContainsKey(NAVBAR_COLOR2_R) ? colorDict[NAVBAR_COLOR2_R] : 165;
                int navbar2G = colorDict.ContainsKey(NAVBAR_COLOR2_G) ? colorDict[NAVBAR_COLOR2_G] : 95;
                int navbar2B = colorDict.ContainsKey(NAVBAR_COLOR2_B) ? colorDict[NAVBAR_COLOR2_B] : 253;
                int navbar3R = colorDict.ContainsKey(NAVBAR_COLOR3_R) ? colorDict[NAVBAR_COLOR3_R] : 16;
                int navbar3G = colorDict.ContainsKey(NAVBAR_COLOR3_G) ? colorDict[NAVBAR_COLOR3_G] : 55;
                int navbar3B = colorDict.ContainsKey(NAVBAR_COLOR3_B) ? colorDict[NAVBAR_COLOR3_B] : 161;
                int sidebarHoverR = colorDict.ContainsKey(SIDEBAR_HOVER_R) ? colorDict[SIDEBAR_HOVER_R] : 255;
                int sidebarHoverG = colorDict.ContainsKey(SIDEBAR_HOVER_G) ? colorDict[SIDEBAR_HOVER_G] : 255;
                int sidebarHoverB = colorDict.ContainsKey(SIDEBAR_HOVER_B) ? colorDict[SIDEBAR_HOVER_B] : 255;
                int sidebarHoverBorderR = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_R) ? colorDict[SIDEBAR_HOVER_BORDER_R] : 165;
                int sidebarHoverBorderG = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_G) ? colorDict[SIDEBAR_HOVER_BORDER_G] : 95;
                int sidebarHoverBorderB = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_B) ? colorDict[SIDEBAR_HOVER_BORDER_B] : 253;
                int module1R = colorDict.ContainsKey(MODULE_COLOR1_R) ? colorDict[MODULE_COLOR1_R] : 87;
                int module1G = colorDict.ContainsKey(MODULE_COLOR1_G) ? colorDict[MODULE_COLOR1_G] : 179;
                int module1B = colorDict.ContainsKey(MODULE_COLOR1_B) ? colorDict[MODULE_COLOR1_B] : 252;
                int module2R = colorDict.ContainsKey(MODULE_COLOR2_R) ? colorDict[MODULE_COLOR2_R] : 165;
                int module2G = colorDict.ContainsKey(MODULE_COLOR2_G) ? colorDict[MODULE_COLOR2_G] : 95;
                int module2B = colorDict.ContainsKey(MODULE_COLOR2_B) ? colorDict[MODULE_COLOR2_B] : 253;
                int module3R = colorDict.ContainsKey(MODULE_COLOR3_R) ? colorDict[MODULE_COLOR3_R] : 16;
                int module3G = colorDict.ContainsKey(MODULE_COLOR3_G) ? colorDict[MODULE_COLOR3_G] : 55;
                int module3B = colorDict.ContainsKey(MODULE_COLOR3_B) ? colorDict[MODULE_COLOR3_B] : 161;

                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    defaults.Sidebar1R, defaults.Sidebar1G, defaults.Sidebar1B,
                    defaults.Sidebar2R, defaults.Sidebar2G, defaults.Sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    defaults.Sidebar1R, defaults.Sidebar1G, defaults.Sidebar1B,
                    defaults.Sidebar2R, defaults.Sidebar2G, defaults.Sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores de la Barra Lateral restablecidos a los valores predeterminados! Recarga la pagina para ver los cambios.", "alert-info");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al restablecer los colores: {ex.Message}", "alert-danger");
            }
        }

        private void ResetModuleColors()
        {
            try
            {
                var defaults = new DefaultColors();

                // Reset only Module colors
                txtModule1R.Text = defaults.Module1R.ToString();
                txtModule1G.Text = defaults.Module1G.ToString();
                txtModule1B.Text = defaults.Module1B.ToString();
                txtModule2R.Text = defaults.Module2R.ToString();
                txtModule2G.Text = defaults.Module2G.ToString();
                txtModule2B.Text = defaults.Module2B.ToString();
                txtModule3R.Text = defaults.Module3R.ToString();
                txtModule3G.Text = defaults.Module3G.ToString();
                txtModule3B.Text = defaults.Module3B.ToString();

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int navbar1R = colorDict.ContainsKey(NAVBAR_COLOR1_R) ? colorDict[NAVBAR_COLOR1_R] : 87;
                int navbar1G = colorDict.ContainsKey(NAVBAR_COLOR1_G) ? colorDict[NAVBAR_COLOR1_G] : 179;
                int navbar1B = colorDict.ContainsKey(NAVBAR_COLOR1_B) ? colorDict[NAVBAR_COLOR1_B] : 252;
                int navbar2R = colorDict.ContainsKey(NAVBAR_COLOR2_R) ? colorDict[NAVBAR_COLOR2_R] : 165;
                int navbar2G = colorDict.ContainsKey(NAVBAR_COLOR2_G) ? colorDict[NAVBAR_COLOR2_G] : 95;
                int navbar2B = colorDict.ContainsKey(NAVBAR_COLOR2_B) ? colorDict[NAVBAR_COLOR2_B] : 253;
                int navbar3R = colorDict.ContainsKey(NAVBAR_COLOR3_R) ? colorDict[NAVBAR_COLOR3_R] : 16;
                int navbar3G = colorDict.ContainsKey(NAVBAR_COLOR3_G) ? colorDict[NAVBAR_COLOR3_G] : 55;
                int navbar3B = colorDict.ContainsKey(NAVBAR_COLOR3_B) ? colorDict[NAVBAR_COLOR3_B] : 161;
                int sidebar1R = colorDict.ContainsKey(SIDEBAR_COLOR1_R) ? colorDict[SIDEBAR_COLOR1_R] : 87;
                int sidebar1G = colorDict.ContainsKey(SIDEBAR_COLOR1_G) ? colorDict[SIDEBAR_COLOR1_G] : 179;
                int sidebar1B = colorDict.ContainsKey(SIDEBAR_COLOR1_B) ? colorDict[SIDEBAR_COLOR1_B] : 252;
                int sidebar2R = colorDict.ContainsKey(SIDEBAR_COLOR2_R) ? colorDict[SIDEBAR_COLOR2_R] : 130;
                int sidebar2G = colorDict.ContainsKey(SIDEBAR_COLOR2_G) ? colorDict[SIDEBAR_COLOR2_G] : 157;
                int sidebar2B = colorDict.ContainsKey(SIDEBAR_COLOR2_B) ? colorDict[SIDEBAR_COLOR2_B] : 245;
                int sidebarHoverR = colorDict.ContainsKey(SIDEBAR_HOVER_R) ? colorDict[SIDEBAR_HOVER_R] : 255;
                int sidebarHoverG = colorDict.ContainsKey(SIDEBAR_HOVER_G) ? colorDict[SIDEBAR_HOVER_G] : 255;
                int sidebarHoverB = colorDict.ContainsKey(SIDEBAR_HOVER_B) ? colorDict[SIDEBAR_HOVER_B] : 255;
                int sidebarHoverBorderR = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_R) ? colorDict[SIDEBAR_HOVER_BORDER_R] : 165;
                int sidebarHoverBorderG = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_G) ? colorDict[SIDEBAR_HOVER_BORDER_G] : 95;
                int sidebarHoverBorderB = colorDict.ContainsKey(SIDEBAR_HOVER_BORDER_B) ? colorDict[SIDEBAR_HOVER_BORDER_B] : 253;

                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    defaults.Module1R, defaults.Module1G, defaults.Module1B,
                    defaults.Module2R, defaults.Module2G, defaults.Module2B,
                    defaults.Module3R, defaults.Module3G, defaults.Module3B,
                    87, 179, 252
                );

                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    sidebarHoverR, sidebarHoverG, sidebarHoverB,
                    sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB,
                    defaults.Module1R, defaults.Module1G, defaults.Module1B,
                    defaults.Module2R, defaults.Module2G, defaults.Module2B,
                    defaults.Module3R, defaults.Module3G, defaults.Module3B,
                    87, 179, 252
                );

                ShowMessage("Colores de los Modulos restablecidos a los valores predeterminados! Recarga la pagina para ver los cambios.", "alert-info");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al restablecer los colores: {ex.Message}", "alert-danger");
            }
        }

        private void ResetSidebarHoverColors()
        {
            try
            {
                var defaults = new DefaultColors();

                // Reset only Sidebar Hover colors
                txtSidebarHoverR.Text = defaults.SidebarHoverR.ToString();
                txtSidebarHoverG.Text = defaults.SidebarHoverG.ToString();
                txtSidebarHoverB.Text = defaults.SidebarHoverB.ToString();
                txtSidebarHoverBorderR.Text = defaults.SidebarHoverBorderR.ToString();
                txtSidebarHoverBorderG.Text = defaults.SidebarHoverBorderG.ToString();
                txtSidebarHoverBorderB.Text = defaults.SidebarHoverBorderB.ToString();

                // Load existing colors for other sections
                var colorDict = LoadColorsFromDatabase();

                int navbar1R = colorDict.ContainsKey(NAVBAR_COLOR1_R) ? colorDict[NAVBAR_COLOR1_R] : 87;
                int navbar1G = colorDict.ContainsKey(NAVBAR_COLOR1_G) ? colorDict[NAVBAR_COLOR1_G] : 179;
                int navbar1B = colorDict.ContainsKey(NAVBAR_COLOR1_B) ? colorDict[NAVBAR_COLOR1_B] : 252;
                int navbar2R = colorDict.ContainsKey(NAVBAR_COLOR2_R) ? colorDict[NAVBAR_COLOR2_R] : 165;
                int navbar2G = colorDict.ContainsKey(NAVBAR_COLOR2_G) ? colorDict[NAVBAR_COLOR2_G] : 95;
                int navbar2B = colorDict.ContainsKey(NAVBAR_COLOR2_B) ? colorDict[NAVBAR_COLOR2_B] : 253;
                int navbar3R = colorDict.ContainsKey(NAVBAR_COLOR3_R) ? colorDict[NAVBAR_COLOR3_R] : 16;
                int navbar3G = colorDict.ContainsKey(NAVBAR_COLOR3_G) ? colorDict[NAVBAR_COLOR3_G] : 55;
                int navbar3B = colorDict.ContainsKey(NAVBAR_COLOR3_B) ? colorDict[NAVBAR_COLOR3_B] : 161;
                int sidebar1R = colorDict.ContainsKey(SIDEBAR_COLOR1_R) ? colorDict[SIDEBAR_COLOR1_R] : 87;
                int sidebar1G = colorDict.ContainsKey(SIDEBAR_COLOR1_G) ? colorDict[SIDEBAR_COLOR1_G] : 179;
                int sidebar1B = colorDict.ContainsKey(SIDEBAR_COLOR1_B) ? colorDict[SIDEBAR_COLOR1_B] : 252;
                int sidebar2R = colorDict.ContainsKey(SIDEBAR_COLOR2_R) ? colorDict[SIDEBAR_COLOR2_R] : 130;
                int sidebar2G = colorDict.ContainsKey(SIDEBAR_COLOR2_G) ? colorDict[SIDEBAR_COLOR2_G] : 157;
                int sidebar2B = colorDict.ContainsKey(SIDEBAR_COLOR2_B) ? colorDict[SIDEBAR_COLOR2_B] : 245;
                int module1R = colorDict.ContainsKey(MODULE_COLOR1_R) ? colorDict[MODULE_COLOR1_R] : 87;
                int module1G = colorDict.ContainsKey(MODULE_COLOR1_G) ? colorDict[MODULE_COLOR1_G] : 179;
                int module1B = colorDict.ContainsKey(MODULE_COLOR1_B) ? colorDict[MODULE_COLOR1_B] : 252;
                int module2R = colorDict.ContainsKey(MODULE_COLOR2_R) ? colorDict[MODULE_COLOR2_R] : 165;
                int module2G = colorDict.ContainsKey(MODULE_COLOR2_G) ? colorDict[MODULE_COLOR2_G] : 95;
                int module2B = colorDict.ContainsKey(MODULE_COLOR2_B) ? colorDict[MODULE_COLOR2_B] : 253;
                int module3R = colorDict.ContainsKey(MODULE_COLOR3_R) ? colorDict[MODULE_COLOR3_R] : 16;
                int module3G = colorDict.ContainsKey(MODULE_COLOR3_G) ? colorDict[MODULE_COLOR3_G] : 55;
                int module3B = colorDict.ContainsKey(MODULE_COLOR3_B) ? colorDict[MODULE_COLOR3_B] : 161;

                SaveColorsToDatabase(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    defaults.SidebarHoverR, defaults.SidebarHoverG, defaults.SidebarHoverB,
                    defaults.SidebarHoverBorderR, defaults.SidebarHoverBorderG, defaults.SidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                GenerateCustomCSS(
                    navbar1R, navbar1G, navbar1B,
                    navbar2R, navbar2G, navbar2B,
                    navbar3R, navbar3G, navbar3B,
                    sidebar1R, sidebar1G, sidebar1B,
                    sidebar2R, sidebar2G, sidebar2B,
                    defaults.SidebarHoverR, defaults.SidebarHoverG, defaults.SidebarHoverB,
                    defaults.SidebarHoverBorderR, defaults.SidebarHoverBorderG, defaults.SidebarHoverBorderB,
                    module1R, module1G, module1B,
                    module2R, module2G, module2B,
                    module3R, module3G, module3B,
                    87, 179, 252
                );

                ShowMessage("Colores del Hover del Sidebar restablecidos a los valores predeterminados! Recarga la pagina para ver los cambios.", "alert-info");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al restablecer los colores: {ex.Message}", "alert-danger");
            }
        }

        private void ResetAllColors()
        {
            try
            {
                var defaults = new DefaultColors();

                txtNavbar1R.Text = defaults.Navbar1R.ToString();
                txtNavbar1G.Text = defaults.Navbar1G.ToString();
                txtNavbar1B.Text = defaults.Navbar1B.ToString();
                txtNavbar2R.Text = defaults.Navbar2R.ToString();
                txtNavbar2G.Text = defaults.Navbar2G.ToString();
                txtNavbar2B.Text = defaults.Navbar2B.ToString();
                txtNavbar3R.Text = defaults.Navbar3R.ToString();
                txtNavbar3G.Text = defaults.Navbar3G.ToString();
                txtNavbar3B.Text = defaults.Navbar3B.ToString();
                txtSidebar1R.Text = defaults.Sidebar1R.ToString();
                txtSidebar1G.Text = defaults.Sidebar1G.ToString();
                txtSidebar1B.Text = defaults.Sidebar1B.ToString();
                txtSidebar2R.Text = defaults.Sidebar2R.ToString();
                txtSidebar2G.Text = defaults.Sidebar2G.ToString();
                txtSidebar2B.Text = defaults.Sidebar2B.ToString();
                txtSidebarHoverR.Text = defaults.SidebarHoverR.ToString();
                txtSidebarHoverG.Text = defaults.SidebarHoverG.ToString();
                txtSidebarHoverB.Text = defaults.SidebarHoverB.ToString();
                txtModule1R.Text = defaults.Module1R.ToString();
                txtModule1G.Text = defaults.Module1G.ToString();
                txtModule1B.Text = defaults.Module1B.ToString();
                txtModule2R.Text = defaults.Module2R.ToString();
                txtModule2G.Text = defaults.Module2G.ToString();
                txtModule2B.Text = defaults.Module2B.ToString();
                txtModule3R.Text = defaults.Module3R.ToString();
                txtModule3G.Text = defaults.Module3G.ToString();
                txtModule3B.Text = defaults.Module3B.ToString();

                SaveColorsToDatabase(
                    defaults.Navbar1R, defaults.Navbar1G, defaults.Navbar1B,
                    defaults.Navbar2R, defaults.Navbar2G, defaults.Navbar2B,
                    defaults.Navbar3R, defaults.Navbar3G, defaults.Navbar3B,
                    defaults.Sidebar1R, defaults.Sidebar1G, defaults.Sidebar1B,
                    defaults.Sidebar2R, defaults.Sidebar2G, defaults.Sidebar2B,
                    defaults.SidebarHoverR, defaults.SidebarHoverG, defaults.SidebarHoverB,
                    defaults.SidebarHoverBorderR, defaults.SidebarHoverBorderG, defaults.SidebarHoverBorderB,
                    defaults.Module1R, defaults.Module1G, defaults.Module1B,
                    defaults.Module2R, defaults.Module2G, defaults.Module2B,
                    defaults.Module3R, defaults.Module3G, defaults.Module3B,
                    defaults.ModuleIconR, defaults.ModuleIconG, defaults.ModuleIconB
                );

                GenerateCustomCSS(
                    defaults.Navbar1R, defaults.Navbar1G, defaults.Navbar1B,
                    defaults.Navbar2R, defaults.Navbar2G, defaults.Navbar2B,
                    defaults.Navbar3R, defaults.Navbar3G, defaults.Navbar3B,
                    defaults.Sidebar1R, defaults.Sidebar1G, defaults.Sidebar1B,
                    defaults.Sidebar2R, defaults.Sidebar2G, defaults.Sidebar2B,
                    defaults.SidebarHoverR, defaults.SidebarHoverG, defaults.SidebarHoverB,
                    defaults.SidebarHoverBorderR, defaults.SidebarHoverBorderG, defaults.SidebarHoverBorderB,
                    defaults.Module1R, defaults.Module1G, defaults.Module1B,
                    defaults.Module2R, defaults.Module2G, defaults.Module2B,
                    defaults.Module3R, defaults.Module3G, defaults.Module3B,
                    defaults.ModuleIconR, defaults.ModuleIconG, defaults.ModuleIconB
                );

                ShowMessage("Colores restablecidos a los valores predeterminados! Recarga la pagina para ver los cambios.", "alert-info");
            }
            catch (Exception ex)
            {
                ShowMessage($"Error al restablecer los colores: {ex.Message}", "alert-danger");
            }
        }

        private int ValidateRGB(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{fieldName} no puede estar vacio");

            if (!int.TryParse(value, out int result))
                throw new ArgumentException($"{fieldName} debe ser un numero valido");

            if (result < 0 || result > 255)
                throw new ArgumentException($"{fieldName} debe estar entre 0 y 255");

            return result;
        }

        private void SaveColorsToDatabase(
            int navbar1R, int navbar1G, int navbar1B,
            int navbar2R, int navbar2G, int navbar2B,
            int navbar3R, int navbar3G, int navbar3B,
            int sidebar1R, int sidebar1G, int sidebar1B,
            int sidebar2R, int sidebar2G, int sidebar2B,
            int sidebarHoverR, int sidebarHoverG, int sidebarHoverB,
            int sidebarHoverBorderR, int sidebarHoverBorderG, int sidebarHoverBorderB,
            int module1R, int module1G, int module1B,
            int module2R, int module2G, int module2B,
            int module3R, int module3G, int module3B,
            int moduleIconR, int moduleIconG, int moduleIconB)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ServerCon"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = ConfigurationManager.ConnectionStrings["LocalCon"]?.ConnectionString;
            }

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var colors = new[]
                    {
                        new { Key = NAVBAR_COLOR1_R, Value = navbar1R.ToString() },
                        new { Key = NAVBAR_COLOR1_G, Value = navbar1G.ToString() },
                        new { Key = NAVBAR_COLOR1_B, Value = navbar1B.ToString() },
                        new { Key = NAVBAR_COLOR2_R, Value = navbar2R.ToString() },
                        new { Key = NAVBAR_COLOR2_G, Value = navbar2G.ToString() },
                        new { Key = NAVBAR_COLOR2_B, Value = navbar2B.ToString() },
                        new { Key = NAVBAR_COLOR3_R, Value = navbar3R.ToString() },
                        new { Key = NAVBAR_COLOR3_G, Value = navbar3G.ToString() },
                        new { Key = NAVBAR_COLOR3_B, Value = navbar3B.ToString() },
                        new { Key = SIDEBAR_COLOR1_R, Value = sidebar1R.ToString() },
                        new { Key = SIDEBAR_COLOR1_G, Value = sidebar1G.ToString() },
                        new { Key = SIDEBAR_COLOR1_B, Value = sidebar1B.ToString() },
                        new { Key = SIDEBAR_COLOR2_R, Value = sidebar2R.ToString() },
                        new { Key = SIDEBAR_COLOR2_G, Value = sidebar2G.ToString() },
                        new { Key = SIDEBAR_COLOR2_B, Value = sidebar2B.ToString() },
                        new { Key = SIDEBAR_HOVER_R, Value = sidebarHoverR.ToString() },
                        new { Key = SIDEBAR_HOVER_G, Value = sidebarHoverG.ToString() },
                        new { Key = SIDEBAR_HOVER_B, Value = sidebarHoverB.ToString() },
                        new { Key = SIDEBAR_HOVER_BORDER_R, Value = sidebarHoverBorderR.ToString() },
                        new { Key = SIDEBAR_HOVER_BORDER_G, Value = sidebarHoverBorderG.ToString() },
                        new { Key = SIDEBAR_HOVER_BORDER_B, Value = sidebarHoverBorderB.ToString() },
                        new { Key = MODULE_COLOR1_R, Value = module1R.ToString() },
                        new { Key = MODULE_COLOR1_G, Value = module1G.ToString() },
                        new { Key = MODULE_COLOR1_B, Value = module1B.ToString() },
                        new { Key = MODULE_COLOR2_R, Value = module2R.ToString() },
                        new { Key = MODULE_COLOR2_G, Value = module2G.ToString() },
                        new { Key = MODULE_COLOR2_B, Value = module2B.ToString() },
                        new { Key = MODULE_COLOR3_R, Value = module3R.ToString() },
                        new { Key = MODULE_COLOR3_G, Value = module3G.ToString() },
                        new { Key = MODULE_COLOR3_B, Value = module3B.ToString() },
                        new { Key = MODULE_ICON_R, Value = moduleIconR.ToString() },
                        new { Key = MODULE_ICON_G, Value = moduleIconG.ToString() },
                        new { Key = MODULE_ICON_B, Value = moduleIconB.ToString() }
                    };

                    string query = @"
                        MERGE ColorSettings AS target
                        USING (VALUES (@key, @value)) AS source (SettingKey, SettingValue)
                        ON target.SettingKey = source.SettingKey
                        WHEN MATCHED THEN 
                            UPDATE SET SettingValue = source.SettingValue, UpdatedDate = GETDATE()
                        WHEN NOT MATCHED THEN
                            INSERT (SettingKey, SettingValue) VALUES (source.SettingKey, source.SettingValue);";

                    foreach (var color in colors)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@key", color.Key);
                            cmd.Parameters.AddWithValue("@value", color.Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void GenerateCustomCSS(
            int navbar1R, int navbar1G, int navbar1B,
            int navbar2R, int navbar2G, int navbar2B,
            int navbar3R, int navbar3G, int navbar3B,
            int sidebar1R, int sidebar1G, int sidebar1B,
            int sidebar2R, int sidebar2G, int sidebar2B,
            int sidebarHoverR, int sidebarHoverG, int sidebarHoverB,
            int sidebarHoverBorderR, int sidebarHoverBorderG, int sidebarHoverBorderB,
            int module1R, int module1G, int module1B,
            int module2R, int module2G, int module2B,
            int module3R, int module3G, int module3B,
            int moduleIconR, int moduleIconG, int moduleIconB)
        {
            // Usar Ticks del reloj para cache-busting
            long cacheVersion = DateTime.Now.Ticks;

            string cssContent = $@"/* Auto-generated custom colors - RGB Format */
/* Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss} */
/* Cache Version: {cacheVersion} */

/* ============================================
   NAVBAR COLORS (3 colors with gradient)
   ============================================ */
.sb-topnav {{
    background: linear-gradient(45deg, rgb({navbar1R}, {navbar1G}, {navbar1B}), rgb({navbar2R}, {navbar2G}, {navbar2B}), rgb({navbar3R}, {navbar3G}, {navbar3B})) !important;
}}

/* ============================================
    SIDEBAR COLORS (2 colors with gradient)
    ============================================ */
.sb-sidenav,
.sb-sidenav.sb-sidenav-dark,
#layoutSidenav_nav,
.sb-sidenav .sb-sidenav-menu,
.sb-sidenav .sb-sidenav-menu .nav-link {{
    background: linear-gradient(90deg, rgb({sidebar1R}, {sidebar1G}, {sidebar1B}), rgb({sidebar2R}, {sidebar2G}, {sidebar2B})) !important;
    color: #ffffff;
}}

.sb-sidenav-footer {{
    background: linear-gradient(90deg, rgb({sidebar1R}, {sidebar1G}, {sidebar1B}), rgb({sidebar2R}, {sidebar2G}, {sidebar2B})) !important;
    color: #ffffff !important;
}}

/* ============================================
   HOVER EFFECTS
   ============================================ */
.sb-sidenav-dark .nav-link:hover {{
    background: linear-gradient(90deg, rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.3), rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.3)), linear-gradient(90deg, rgb({sidebar1R}, {sidebar1G}, {sidebar1B}), rgb({sidebar2R}, {sidebar2G}, {sidebar2B})) !important;
    border-left: 4px solid rgb({sidebarHoverBorderR}, {sidebarHoverBorderG}, {sidebarHoverBorderB}) !important;
    color: #ffffff;
}}

.sb-sidenav .nav-link:hover {{
    background: linear-gradient(90deg, rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.2), rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.2)), linear-gradient(90deg, rgb({sidebar1R}, {sidebar1G}, {sidebar1B}), rgb({sidebar2R}, {sidebar2G}, {sidebar2B})) !important;
    border-left: 4px solid rgb({sidebarHoverBorderR}, {sidebarHoverBorderG}, {sidebarHoverBorderB}) !important;
    color: #ffffff;
}}

/* ============================================
   TOOLTIPS
   ============================================ */
.tooltip-inner {{
    background-color: rgb({navbar3R}, {navbar3G}, {navbar3B}) !important;
    border-radius: 6px;
    padding: 8px 12px;
    font-size: 0.875rem;
    font-weight: 500;
}}

.tooltip.bs-tooltip-right .tooltip-arrow::before {{
    border-right-color: rgb({navbar3R}, {navbar3G}, {navbar3B}) !important;
}}

/* ============================================
   SIDEBAR ICONS
   ============================================ */
.sb-nav-link-icon {{
    color: rgb({module3R}, {module3G}, {module3B}) !important;
}}

/* ============================================
   MENU ITEMS - Collapsed and Nested
   ============================================ */
.sb-sidenav-menu-nested .nav-link:hover {{
    background: linear-gradient(90deg, rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.15), rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.15)), linear-gradient(90deg, rgb({sidebar1R}, {sidebar1G}, {sidebar1B}), rgb({sidebar2R}, {sidebar2G}, {sidebar2B})) !important;
    color: #ffffff;
}}

a.nav-link.collapsed:hover {{
    background: linear-gradient(90deg, rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.2), rgba({sidebarHoverR}, {sidebarHoverG}, {sidebarHoverB}, 0.2)), linear-gradient(90deg, rgb({sidebar1R}, {sidebar1G}, {sidebar1B}), rgb({sidebar2R}, {sidebar2G}, {sidebar2B})) !important;
    border-left: 4px solid rgb({sidebarHoverBorderR}, {sidebarHoverBorderG}, {sidebarHoverBorderB}) !important;
    color: #ffffff;
}}";

            string cssPath = Server.MapPath("~/Content/custom-colors.css");
            File.WriteAllText(cssPath, cssContent);

            // Generate module colors CSS
            string moduleCssContent = $@"/* Auto-generated module colors - RGB Format */
/* Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss} */

/* ============================================
   MODULE CARD COLORS (Solid borders)
   ============================================ */

a.module-card {{
    border-color: rgb({module1R}, {module1G}, {module1B}) !important;
    background: #ffffff !important;
}}

a.module-card:hover {{
    border-color: rgb({module2R}, {module2G}, {module2B}) !important;
    background: rgb({module3R}, {module3G}, {module3B}) !important;
}}

.module-icon {{
    color: rgb({module2R}, {module2G}, {module2B}) !important;
}}

a.module-card:hover .module-icon {{
    color: rgb({module1R}, {module1G}, {module1B}) !important;
}}

.module-title {{
    color: rgb({module3R}, {module3G}, {module3B}) !important;
}}

a.module-card:hover .module-title {{
    color: rgb({module1R}, {module1G}, {module1B}) !important;
}}

.module-description {{
    color: rgb({module3R}, {module3G}, {module3B}) !important;
}}

a.module-card:hover .module-description {{
    color: rgb({module2R}, {module2G}, {module2B}) !important;
}}";

            string moduleCssPath = Server.MapPath("~/Content/module-colors.css");
            File.WriteAllText(moduleCssPath, moduleCssContent);

            // Clear browser cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
            Response.Cache.SetMaxAge(TimeSpan.Zero);
            Response.AppendHeader("Pragma", "no-cache");
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert {cssClass}";
            lblMessage.Visible = true;
        }
    }
}
