using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Services;
using WebApp.Attributes;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class InventarioController : Controller
    {
        private readonly ILogger<InventarioController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPermissionService _permissionService;
        private readonly string _strcon;

        public InventarioController(
            ILogger<InventarioController> logger, 
            IConfiguration configuration,
            IPermissionService permissionService)
        {
            _logger = logger;
            _configuration = configuration;
            _permissionService = permissionService;
            _strcon = _configuration.GetConnectionString("ServerCon") ?? throw new InvalidOperationException("Connection string 'ServerCon' not found.");
        }

        [HttpGet]
        [RequirePermission(SubmoduleNames.Productos, "Ver")]
        public async Task<IActionResult> Productos(
      string searchTerm = "",
      string sortExpression = "ProductoCompleto",
      string sortDirection = "ASC",
      int page = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // UI permissions   public const string Producto = "Inventario.Productos";
            ViewBag.CanCreate = await _permissionService
                .UserHasPermissionAsync(userId, SubmoduleNames.Productos, "Crear");

            ViewBag.CanEdit = await _permissionService
                .UserHasPermissionAsync(userId, SubmoduleNames.Productos   , "Editar");

            ViewBag.CanDelete = await _permissionService
                .UserHasPermissionAsync(userId, SubmoduleNames.Productos, "Eliminar");

            ViewData["Title"] = "Productos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Catálogo", null)
            };

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortExpression = sortExpression;
            ViewBag.SortDirection = sortDirection;
            ViewBag.CurrentPage = page;

            var productos = ObtenerProductos(searchTerm, sortExpression, sortDirection);
            return View("~/Views/Modules/Productos.cshtml", productos);
        }

        private List<Producto> ObtenerProductos(string searchTerm = "", string sortExpression = "ProductoCompleto", string sortDirection = "ASC")
        {
            var productos = new List<Producto>();

            string sql = @"
                SELECT TOP (1000)
                       Id,
                       Producto,
                       Talla,
                       LTRIM(RTRIM(
                           CASE 
                               WHEN (Producto IS NULL OR LTRIM(RTRIM(Producto)) = '')
                                    AND (Talla IS NULL OR LTRIM(RTRIM(Talla)) = '') THEN ''
                               WHEN (Talla IS NULL OR LTRIM(RTRIM(Talla)) = '') THEN Producto
                               WHEN (Producto IS NULL OR LTRIM(RTRIM(Producto)) = '') THEN Talla
                               ELSE Producto + ' ' + Talla
                           END
                       )) AS ProductoCompleto
                FROM hd.Catalogo
                WHERE (Producto IS NOT NULL AND LTRIM(RTRIM(Producto)) <> '')
            ";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sql += @"
                  AND (
                        (Producto IS NOT NULL AND Producto LIKE @q)
                     OR (Talla IS NOT NULL AND Talla LIKE @q)
                     OR ((Producto IS NOT NULL AND Talla IS NOT NULL) AND (Producto + ' ' + Talla) LIKE @q)
                     OR (Id LIKE @q)
                  )";
            }

            string safeSort = "ProductoCompleto";
            if (!string.IsNullOrEmpty(sortExpression) &&
                (sortExpression.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                 sortExpression.Equals("ProductoCompleto", StringComparison.OrdinalIgnoreCase)))
            {
                safeSort = sortExpression;
            }
            string safeDir = string.Equals(sortDirection, "DESC", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
            sql += $" ORDER BY {safeSort} {safeDir}";

            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    cmd.Parameters.Add(new SqlParameter("@q", SqlDbType.NVarChar, 200) { Value = $"%{searchTerm.Trim()}%" });

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            Id = reader["Id"]?.ToString() ?? string.Empty,
                            ProductoNombre = reader["Producto"]?.ToString() ?? string.Empty,
                            Talla = reader["Talla"]?.ToString() ?? string.Empty,
                            ProductoCompleto = reader["ProductoCompleto"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }

            return productos;
        }


        [HttpPost]
        [RequirePermission("Inventario.Productos", "Editar")] 
        public IActionResult Guardar(string id, string producto, string talla, string originalId = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(producto))
                {
                    return Json(new { success = false, message = "El campo Id y Producto son obligatorios." });
                }

                if (string.IsNullOrEmpty(originalId))
                {
                    Insertar(id, producto, talla);
                    return Json(new { success = true, message = "Producto agregado correctamente." });
                }
                else
                {
                    Actualizar(originalId, producto, talla);
                    return Json(new { success = true, message = "Producto actualizado correctamente." });
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Json(new { success = false, message = "El Id ya existe. Usa un Id diferente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        [RequirePermission("Inventario.Productos", "Eliminar")]
        public IActionResult Eliminar(string id)

        {
            try
            {
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand("DELETE FROM hd.Catalogo WHERE Id=@Id;", con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = id;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true, message = "Producto eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        [HttpGet]
        [RequirePermission("Inventario.Productos", "Ver")]
        public IActionResult ObtenerProducto(string id)

        {
            try
            {
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand("SELECT Id, Producto, Talla FROM hd.Catalogo WHERE Id=@Id", con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = id;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var producto = new
                            {
                                id = reader["Id"]?.ToString() ?? string.Empty,
                                producto = reader["Producto"]?.ToString() ?? string.Empty,
                                talla = reader["Talla"]?.ToString() ?? string.Empty
                            };
                            return Json(new { success = true, data = producto });
                        }
                    }
                }
                return Json(new { success = false, message = "Producto no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        private void Insertar(string id, string producto, string talla)
        {
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(
                "INSERT INTO hd.Catalogo (Id, Producto, Talla) VALUES (@Id, @Producto, @Talla);", con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = id;
                cmd.Parameters.Add("@Producto", SqlDbType.NVarChar, 150).Value = (object)producto ?? DBNull.Value;
                cmd.Parameters.Add("@Talla", SqlDbType.NVarChar, 50).Value = (object)talla ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void Actualizar(string id, string producto, string talla)
        {
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(
                "UPDATE hd.Catalogo SET Producto=@Producto, Talla=@Talla WHERE Id=@Id;", con))
            {
                cmd.Parameters.Add("@Producto", SqlDbType.NVarChar, 150).Value = (object)producto ?? DBNull.Value;
                cmd.Parameters.Add("@Talla", SqlDbType.NVarChar, 50).Value = (object)talla ?? DBNull.Value;
                cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = id;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IActionResult Stock()
        {
            ViewData["Title"] = "Stock";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Stock", null)
            };
            return View("~/Views/Modules/Stock.cshtml");
        }

        public IActionResult Almacenes()
        {
            ViewData["Title"] = "Almacenes";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Almacenes", null)
            };
            return View("~/Views/Modules/Almacenes.cshtml");
        }

        public IActionResult Categorias()
        {
            ViewData["Title"] = "Categorías";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Categorías", null)
            };
            return View("~/Views/Modules/Categorias.cshtml");
        }

        public IActionResult Movimientos()
        {
            ViewData["Title"] = "Movimientos de Inventario";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Movimientos", null)
            };
            return View("~/Views/Modules/Movimientos.cshtml");
        }

        public IActionResult AuditoriaInventario()
        {
            ViewData["Title"] = "Auditoría de Inventario";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Auditoría", null)
            };
            return View("~/Views/Modules/AuditoriaInventario.cshtml");
        }

        public IActionResult AjustesInventario()
        {
            ViewData["Title"] = "Ajustes de Inventario";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Ajustes", null)
            };
            return View("~/Views/Modules/AjustesInventario.cshtml");
        }

        public IActionResult ReportesInventario()
        {
            ViewData["Title"] = "Reportes de Inventario";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Reportes", null)
            };
            return View("~/Views/Modules/ReportesInventario.cshtml");
        }


        public IActionResult Catalogo()
        {
            ViewData["Title"] = "Catálogo";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Inventario", Url.Action("Inventario", "Modules")),
                ("Catalogo", null)
            };
            return View("~/Views/Modules/Catalogo.cshtml");
        }
    }
}
