using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using UserRoles.Identity.Constants;
using UserRoles.Identity.Services;
using WebApp.Attributes;
using WebApp.Models; 

namespace WebApp.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly ILogger<ComprasController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPermissionService _permissionService;


        private const long MaxFileSize = 1 * 1024 * 1024; // 30 MB

        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".gif",".bmp",
            ".xls", ".xlsx",
            ".doc", ".docx",
            ".pdf",
            ".txt"
        };


        public ComprasController(
            ILogger<ComprasController> logger, 
            IConfiguration configuration,
            IPermissionService permissionService)
        {
            _logger = logger;
            _configuration = configuration;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!await _permissionService.UserHasModuleAccessAsync(userId, ModuleNames.Compras))
                return Forbid();

            return View();
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ServerCon");
        }

        public async Task<IActionResult> ComprasInternacionales(string searchTerm = null, int page = 1)
        {
            ViewData["Title"] = "Compras Internacionales";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules")),
                ("Compras Internacionales", null)
            };
            ViewBag.SearchTerm = searchTerm;

            // Obtener permisos del usuario para habilitar/deshabilitar botones
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var canView = true; // Ver siempre habilitado para usuarios autenticados
            var canEdit = await _permissionService.UserHasPermissionAsync(userId, "Compras.ComprasInternacionales", "Editar");
            var canDelete = await _permissionService.UserHasPermissionAsync(userId, "Compras.ComprasInternacionales", "Eliminar");

            ViewBag.CanView = canView;
            ViewBag.CanEdit = canEdit;
            ViewBag.CanDelete = canDelete;

            int pageSize = 3;
            if (page < 1) page = 1;

            var allCompras = ObtenerComprasInternacionales(searchTerm);
            int totalCount = allCompras.Count;
            int totalPages = (totalCount + pageSize - 1) / pageSize;

            if (page > totalPages && totalPages > 0)
                page = totalPages;

            var comprasPage = allCompras
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View("~/Views/Modules/ComprasInternacionales.cshtml", (comprasPage, totalCount, page, pageSize));
        }

        private List<CompraInternacional> ObtenerComprasInternacionales(string filtro = null)
        {
            var compras = new List<CompraInternacional>();
            using (var con = new SqlConnection(GetConnectionString()))
            using (var cmd = new SqlCommand(@"
                SELECT Id, FechaCompra, Contract, Container, Sello, Proveedor, 
                       Origen, Puerto, Naviera, ETD, ETA, Cruce, Incoterm,
                       Invoice, Pedimento, ConexionesDemoras, FechaEstimDestino, UltimoDiaLibre, EntregaVacio, DiasDemoras,
                       FacturaDemoras, MontoUSDDemoras, TCDemoras, MontoDemorasMXN,
                       FacturaComercializadora, FechaDespacho, FechaPagoPedimento, Planta,
                       Lote, FechaProduccion, FechaCaducidad, Empresa, Codigo, DescripcionProducto, Producto, Marca, Talla, Kgs, Cajas
                FROM hd.Carga 
                WHERE (@filtro IS NULL OR 
                       Contract LIKE '%'+@filtro+'%' OR 
                       Container LIKE '%'+@filtro+'%' OR 
                       Proveedor LIKE '%'+@filtro+'%') 
                ORDER BY Id DESC;", con))
            {
                cmd.Parameters.Add("@filtro", System.Data.SqlDbType.NVarChar, 200).Value =
                    string.IsNullOrWhiteSpace(filtro) ? (object)DBNull.Value : filtro.Trim();

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compras.Add(new CompraInternacional
                        {
                            Id = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader[0]),

                            FechaCompra = reader.IsDBNull("FechaCompra") ? null: DateOnly.FromDateTime(reader.GetDateTime("FechaCompra")),
                            Contract = reader.IsDBNull(2) ? string.Empty : reader[2].ToString(),
                            Container = reader.IsDBNull(3) ? string.Empty : reader[3].ToString(),
                            Sello = reader.IsDBNull(4) ? string.Empty : reader[4].ToString(),
                            Proveedor = reader.IsDBNull(5) ? string.Empty : reader[5].ToString(),
                            Origen = reader.IsDBNull(6) ? string.Empty : reader[6].ToString(),
                            Puerto = reader.IsDBNull(7) ? string.Empty : reader[7].ToString(),
                            Naviera = reader.IsDBNull(8) ? string.Empty : reader[8].ToString(),
                            ETD = reader.IsDBNull(9) ? (DateTime?)null : Convert.ToDateTime(reader[9]),
                            ETA = reader.IsDBNull(10) ? (DateTime?)null : Convert.ToDateTime(reader[10]),
                            Cruce = reader.IsDBNull(11) ? string.Empty : reader[11].ToString(),
                            Incoterm = reader.IsDBNull(12) ? string.Empty : reader[12].ToString(),
                            Invoice = reader.IsDBNull(13) ? string.Empty : reader[13].ToString(),
                            Pedimento = reader.IsDBNull(14) ? string.Empty : reader[14].ToString(),
                            ConexionesDemoras = reader.IsDBNull(15) ? string.Empty : reader[15].ToString(),
                            FechaEstimDestino = reader.IsDBNull(16) ? (DateTime?)null : Convert.ToDateTime(reader[16]),
                            UltimoDiaLibre = reader.IsDBNull(17) ? (DateTime?)null : Convert.ToDateTime(reader[17]),
                            EntregaVacio = reader.IsDBNull(18) ? (DateTime?)null : Convert.ToDateTime(reader[18]),
                            DiasDemoras = reader.IsDBNull(19) ? (int?)null : Convert.ToInt32(reader[19]),
                            FacturaDemoras = reader.IsDBNull(20) ? string.Empty : reader[20].ToString(),
                            MontoUSDDemoras = reader.IsDBNull(21) ? (decimal?)null : Convert.ToDecimal(reader[21]),
                            TCDemoras = reader.IsDBNull(22) ? (decimal?)null : Convert.ToDecimal(reader[22]),
                            MontoDemorasMXN = reader.IsDBNull(23) ? (decimal?)null : Convert.ToDecimal(reader[23]),
                            FacturaComercializadora = reader.IsDBNull(24) ? string.Empty : reader[24].ToString(),
                            FechaDespacho = reader.IsDBNull(25) ? (DateTime?)null : Convert.ToDateTime(reader[25]),
                            FechaPagoPedimento = reader.IsDBNull(26) ? (DateTime?)null : Convert.ToDateTime(reader[26]),
                            Planta = reader.IsDBNull(27) ? (int?)null : Convert.ToInt32(reader[27]),
                            Lote = reader.IsDBNull(28) ? string.Empty : reader[28].ToString(),
                            FechaProduccion = reader.IsDBNull(29) ? (DateTime?)null : Convert.ToDateTime(reader[29]),
                            FechaCaducidad = reader.IsDBNull(30) ? (DateTime?)null : Convert.ToDateTime(reader[30]),
                            Empresa = reader.IsDBNull(31) ? string.Empty : reader[31].ToString(),
                            Codigo = reader.IsDBNull(32) ? string.Empty : reader[32].ToString(),
                            DescripcionProducto = reader.IsDBNull(33) ? string.Empty : reader[33].ToString(),
                            Producto = reader.IsDBNull(34) ? string.Empty : reader[34].ToString(),
                            Marca = reader.IsDBNull(35) ? string.Empty : reader[35].ToString(),
                            Talla = reader.IsDBNull(36) ? string.Empty : reader[36].ToString(),
                            Kgs = reader.IsDBNull(37) ? (decimal?)null : Convert.ToDecimal(reader[37]),
                            Cajas = reader.IsDBNull(38) ? (int?)null : Convert.ToInt32(reader[38])
                        });
                    }
                }
            }
            return compras;
        }

        [HttpPost]
        [RequirePermission("Compras.ComprasInternacionales", "Editar")]
        public JsonResult GuardarCompra(int? id, string fechaCompra, string contract, string container,
                                       string sello, string proveedor, string origen, string puerto, 
                                       string naviera, string etd, string eta, string cruce, string incoterm,
                                       string invoice, string pedimento, string conexionesDemoras,
                                       string fechaEstimDestino, string ultimoDiaLibre, string entregaVacio, 
                                       int? diasDemoras, string facturaDemoras, decimal? montoUSDDemoras, 
                                       decimal? tcDemoras, decimal? montoDemorasMXN, string facturaComercializadora, 
                                       string fechaDespacho, string fechaPagoPedimento, int? planta,
                                       string lote, string fechaProduccion, string fechaCaducidad, string empresa, 
                                       string codigo, string descripcionProducto, string producto, string marca, 
                                       string talla, decimal? kgs, int? cajas)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(fechaCompra) || string.IsNullOrWhiteSpace(contract) || 
                    string.IsNullOrWhiteSpace(container) || string.IsNullOrWhiteSpace(proveedor))
                {
                    return Json(new { success = false, message = "Los campos requeridos no pueden estar vacíos." });
                }

                var dtFechaCompra = DateTime.ParseExact(
                    fechaCompra,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture
                );

                DateTime? dtETD = string.IsNullOrWhiteSpace(etd) ? (DateTime?)null : DateTime.Parse(etd);
                DateTime? dtETA = string.IsNullOrWhiteSpace(eta) ? (DateTime?)null : DateTime.Parse(eta);
                DateTime? dtFechaEstimDestino = string.IsNullOrWhiteSpace(fechaEstimDestino) ? (DateTime?)null : DateTime.Parse(fechaEstimDestino);
                DateTime? dtUltimoDiaLibre = string.IsNullOrWhiteSpace(ultimoDiaLibre) ? (DateTime?)null : DateTime.Parse(ultimoDiaLibre);
                DateTime? dtEntregaVacio = string.IsNullOrWhiteSpace(entregaVacio) ? (DateTime?)null : DateTime.Parse(entregaVacio);
                DateTime? dtFechaDespacho = string.IsNullOrWhiteSpace(fechaDespacho) ? (DateTime?)null : DateTime.Parse(fechaDespacho);
                DateTime? dtFechaPagoPedimento = string.IsNullOrWhiteSpace(fechaPagoPedimento) ? (DateTime?)null : DateTime.Parse(fechaPagoPedimento);
                DateTime? dtFechaProduccion = string.IsNullOrWhiteSpace(fechaProduccion) ? (DateTime?)null : DateTime.Parse(fechaProduccion);
                DateTime? dtFechaCaducidad = string.IsNullOrWhiteSpace(fechaCaducidad) ? (DateTime?)null : DateTime.Parse(fechaCaducidad);

                using (var con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    SqlCommand cmd;

                    if (id.HasValue && id.Value > 0)
                    {
                        // UPDATE
                        cmd = new SqlCommand(@"
                            UPDATE hd.Carga 
                            SET FechaCompra=@FechaCompra, Contract=@Contract, Container=@Container,
                                Sello=@Sello, Proveedor=@Proveedor, Origen=@Origen, Puerto=@Puerto,
                                Naviera=@Naviera, ETD=@ETD, ETA=@ETA, Cruce=@Cruce, Incoterm=@Incoterm,
                                Invoice=@Invoice, Pedimento=@Pedimento, ConexionesDemoras=@ConexionesDemoras,
                                FechaEstimDestino=@FechaEstimDestino, UltimoDiaLibre=@UltimoDiaLibre, EntregaVacio=@EntregaVacio,
                                DiasDemoras=@DiasDemoras, FacturaDemoras=@FacturaDemoras, MontoUSDDemoras=@MontoUSDDemoras,
                                TCDemoras=@TCDemoras, MontoDemorasMXN=@MontoDemorasMXN, FacturaComercializadora=@FacturaComercializadora,
                                FechaDespacho=@FechaDespacho, FechaPagoPedimento=@FechaPagoPedimento, Planta=@Planta,
                                Lote=@Lote, FechaProduccion=@FechaProduccion, FechaCaducidad=@FechaCaducidad, Empresa=@Empresa,
                                Codigo=@Codigo, DescripcionProducto=@DescripcionProducto, Producto=@Producto, Marca=@Marca,
                                Talla=@Talla, Kgs=@Kgs, Cajas=@Cajas
                            WHERE Id=@Id", con);

                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id.Value;
                    }
                    else
                    {
                        // INSERT
                        cmd = new SqlCommand(@"
                            INSERT INTO hd.Carga 
                            (FechaCompra, Contract, Container, Sello, Proveedor, Origen, Puerto, 
                             Naviera, ETD, ETA, Cruce, Incoterm, Invoice, Pedimento, ConexionesDemoras,
                             FechaEstimDestino, UltimoDiaLibre, EntregaVacio, DiasDemoras, FacturaDemoras,
                             MontoUSDDemoras, TCDemoras, MontoDemorasMXN, FacturaComercializadora, FechaDespacho,
                             FechaPagoPedimento, Planta, Lote, FechaProduccion, FechaCaducidad, Empresa,
                             Codigo, DescripcionProducto, Producto, Marca, Talla, Kgs, Cajas)
                            VALUES (@FechaCompra, @Contract, @Container, @Sello, @Proveedor, @Origen, 
                                   @Puerto, @Naviera, @ETD, @ETA, @Cruce, @Incoterm, @Invoice, @Pedimento,
                                   @ConexionesDemoras, @FechaEstimDestino, @UltimoDiaLibre, @EntregaVacio,
                                   @DiasDemoras, @FacturaDemoras, @MontoUSDDemoras, @TCDemoras, @MontoDemorasMXN,
                                   @FacturaComercializadora, @FechaDespacho, @FechaPagoPedimento, @Planta,
                                   @Lote, @FechaProduccion, @FechaCaducidad, @Empresa, @Codigo,
                                   @DescripcionProducto, @Producto, @Marca, @Talla, @Kgs, @Cajas)", con);
                    }

                    cmd.Parameters.Add("@FechaCompra", System.Data.SqlDbType.DateTime).Value = dtFechaCompra.Date;
                    cmd.Parameters.Add("@Contract", System.Data.SqlDbType.NVarChar, 40).Value = contract;
                    cmd.Parameters.Add("@Container", System.Data.SqlDbType.NVarChar, 40).Value = container;
                    cmd.Parameters.Add("@Sello", System.Data.SqlDbType.NVarChar, 40).Value = sello ?? string.Empty;
                    cmd.Parameters.Add("@Proveedor", System.Data.SqlDbType.NVarChar, 100).Value = proveedor;
                    cmd.Parameters.Add("@Origen", System.Data.SqlDbType.NVarChar, 100).Value = origen ?? string.Empty;
                    cmd.Parameters.Add("@Puerto", System.Data.SqlDbType.NVarChar, 100).Value = puerto ?? string.Empty;
                    cmd.Parameters.Add("@Naviera", System.Data.SqlDbType.NVarChar, 100).Value = naviera ?? string.Empty;
                    cmd.Parameters.Add("@ETD", System.Data.SqlDbType.DateTime).Value = (object)dtETD ?? DBNull.Value;
                    cmd.Parameters.Add("@ETA", System.Data.SqlDbType.DateTime).Value = (object)dtETA ?? DBNull.Value;
                    cmd.Parameters.Add("@Cruce", System.Data.SqlDbType.NVarChar, 10).Value = cruce ?? string.Empty;
                    cmd.Parameters.Add("@Incoterm", System.Data.SqlDbType.NVarChar, 10).Value = incoterm ?? string.Empty;
                    cmd.Parameters.Add("@Invoice", System.Data.SqlDbType.NVarChar, 40).Value = invoice ?? string.Empty;
                    cmd.Parameters.Add("@Pedimento", System.Data.SqlDbType.NVarChar, 30).Value = pedimento ?? string.Empty;
                    cmd.Parameters.Add("@ConexionesDemoras", System.Data.SqlDbType.NVarChar, 40).Value = conexionesDemoras ?? string.Empty;
                    cmd.Parameters.Add("@FechaEstimDestino", System.Data.SqlDbType.DateTime).Value = (object)dtFechaEstimDestino ?? DBNull.Value;
                    cmd.Parameters.Add("@UltimoDiaLibre", System.Data.SqlDbType.DateTime).Value = (object)dtUltimoDiaLibre ?? DBNull.Value;
                    cmd.Parameters.Add("@EntregaVacio", System.Data.SqlDbType.DateTime).Value = (object)dtEntregaVacio ?? DBNull.Value;
                    cmd.Parameters.Add("@DiasDemoras", System.Data.SqlDbType.Int).Value = (object)diasDemoras ?? DBNull.Value;
                    cmd.Parameters.Add("@FacturaDemoras", System.Data.SqlDbType.NVarChar, 40).Value = facturaDemoras ?? string.Empty;
                    cmd.Parameters.Add("@MontoUSDDemoras", System.Data.SqlDbType.Decimal).Value = (object)montoUSDDemoras ?? DBNull.Value;
                    cmd.Parameters.Add("@TCDemoras", System.Data.SqlDbType.Decimal).Value = (object)tcDemoras ?? DBNull.Value;
                    cmd.Parameters.Add("@MontoDemorasMXN", System.Data.SqlDbType.Decimal).Value = (object)montoDemorasMXN ?? DBNull.Value;
                    cmd.Parameters.Add("@FacturaComercializadora", System.Data.SqlDbType.NVarChar, 40).Value = facturaComercializadora ?? string.Empty;
                    cmd.Parameters.Add("@FechaDespacho", System.Data.SqlDbType.DateTime).Value = (object)dtFechaDespacho ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaPagoPedimento", System.Data.SqlDbType.DateTime).Value = (object)dtFechaPagoPedimento ?? DBNull.Value;
                    cmd.Parameters.Add("@Planta", System.Data.SqlDbType.Int).Value = (object)planta ?? DBNull.Value;
                    cmd.Parameters.Add("@Lote", System.Data.SqlDbType.NVarChar, 50).Value = lote ?? string.Empty;
                    cmd.Parameters.Add("@FechaProduccion", System.Data.SqlDbType.DateTime).Value = (object)dtFechaProduccion ?? DBNull.Value;
                    cmd.Parameters.Add("@FechaCaducidad", System.Data.SqlDbType.DateTime).Value = (object)dtFechaCaducidad ?? DBNull.Value;
                    cmd.Parameters.Add("@Empresa", System.Data.SqlDbType.NVarChar, 100).Value = empresa ?? string.Empty;
                    cmd.Parameters.Add("@Codigo", System.Data.SqlDbType.NVarChar, 50).Value = codigo ?? string.Empty;
                    cmd.Parameters.Add("@DescripcionProducto", System.Data.SqlDbType.NVarChar, 500).Value = descripcionProducto ?? string.Empty;
                    cmd.Parameters.Add("@Producto", System.Data.SqlDbType.NVarChar, 200).Value = producto ?? string.Empty;
                    cmd.Parameters.Add("@Marca", System.Data.SqlDbType.NVarChar, 100).Value = marca ?? string.Empty;
                    cmd.Parameters.Add("@Talla", System.Data.SqlDbType.NVarChar, 50).Value = talla ?? string.Empty;
                    cmd.Parameters.Add("@Kgs", System.Data.SqlDbType.Decimal).Value = (object)kgs ?? DBNull.Value;
                    cmd.Parameters.Add("@Cajas", System.Data.SqlDbType.Int).Value = (object)cajas ?? DBNull.Value;

                    cmd.ExecuteNonQuery();
                }

                string mensaje = id.HasValue && id.Value > 0 ? "Compra actualizada exitosamente." : "Compra guardada exitosamente.";
                return Json(new { success = true, message = mensaje });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar compra internacional");
                return Json(new { success = false, message = "Error al guardar la compra: " + ex.Message });
            }
        }
        //////////////////////////////////////////////////////////////////////////////
        ///  para subir archivos relacionados a cada compra internacional, 
        ///  se guardarán en wwwroot/uploaded/{contract}/filename.ext
        ///  /////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string contract)
        {
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "Archivo inválido." });

            if (file.Length > MaxFileSize)
                return Json(new { success = false, message = "El archivo excede el tamaño máximo de 30 MB." });

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return Json(new
                {
                    success = false,
                    message = "Tipo de archivo no permitido. Solo imágenes, Excel, Word , PDF y TXT."
                });

            if (string.IsNullOrWhiteSpace(contract))
                return Json(new { success = false, message = "Contrato inválido." });

            var safeContract = Path.GetFileName(contract.Trim());
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploaded");
            var contractPath = Path.Combine(basePath, safeContract);

            if (!Directory.Exists(contractPath))
                Directory.CreateDirectory(contractPath);

            var safeFileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(contractPath, safeFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var files = GetFilesForContract(contract).ToList();

            return Json(new { success = true, files });
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult GetFiles(string contract)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return Json(GetFilesForContract(contract));
        }

        private IEnumerable<object> GetFilesForContract(string contract)
        {
            if (string.IsNullOrWhiteSpace(contract))
                return Enumerable.Empty<object>();
             
            var safeContract = Path.GetFileName(contract?.Trim());
            var folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot", "uploaded", safeContract);

            _logger.LogInformation($"FILES FOLDER: [{safeContract}]");

            if (!Directory.Exists(folder))
                return Enumerable.Empty<object>();

            return Directory.GetFiles(folder)
                .Select(f => new
                {
                    name = Path.GetFileName(f),

                    size = new FileInfo(f).Length,
                    url = Url.Content($"~/uploaded/{safeContract}/{Path.GetFileName(f)}")
                })
                .ToList();
        }



        [HttpPost]
        public IActionResult DeleteFile(string contract, string fileName)
        {
            var safeContract = Path.GetFileName(contract);
            var safeFile = Path.GetFileName(fileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", "uploaded", safeContract, safeFile);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            return Json(new { success = true });
        }


        /// /////////////////////////////////////////////////////////  
        /// fin de funcionalidad subir archivos relacionados a cada compra internacional  
        /////////////////////////////////////////////////////////////

        [HttpPost]
        [RequirePermission("Compras.ComprasInternacionales", "Eliminar")]
        public JsonResult EliminarCompra(int id)
        {
            try
            {
                using (var con = new SqlConnection(GetConnectionString()))
                using (var cmd = new SqlCommand("DELETE FROM hd.Carga WHERE Id=@Id", con))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true, message = "Compra eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar compra internacional");
                return Json(new { success = false, message = "Error al eliminar la compra." });
            }
        }

        public IActionResult Proveedores()
        {
            ViewData["Title"] = "Proveedores";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules")),
                ("Proveedores", null)
            };
            return View("~/Views/Modules/Proveedores.cshtml");
        }

        public IActionResult Solicitudes()
        {
            ViewData["Title"] = "Solicitudes";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules")),
                ("Solicitudes", null)
            };
            return View("~/Views/Modules/Solicitudes.cshtml");
        }

        public IActionResult OrdenesCompra()
        {
            ViewData["Title"] = "Órdenes de Compra";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules")),
                ("Órdenes de Compra", null)
            };
            return View("~/Views/Modules/OrdenesCompra.cshtml");
        }

        public IActionResult ReportesCompras()
        {
            ViewData["Title"] = "Reportes de Compras";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules")),
                ("Reportes", null)
            };
            return View("~/Views/Modules/ReportesCompras.cshtml");
        }

        public IActionResult Contratos()
        {
            ViewData["Title"] = "Contratos";
            ViewData["Breadcrumbs"] = new List<(string, string)>
            {
                ("Inicio", Url.Action("Index", "Home")),
                ("Compras", Url.Action("Compras", "Modules")),
                ("Contratos", null)
            };
            return View("~/Views/Modules/Contratos.cshtml");
        }
    }
}
