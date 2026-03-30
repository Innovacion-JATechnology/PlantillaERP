using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
 
    namespace Maqueta
{
    public partial class ComprasInternacionales : System.Web.UI.Page



    //public partial class capturaCargas : System.Web.UI.Page
    {
        private readonly string _strcon = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                BindGrid();
                // Popular listas fijas
                CargarListas();
                CargarListaProductos();
                // Opcional: posiciona en el registro más reciente si existe 
                //   var lastId = GetLastId();
                // if (lastId.HasValue) LoadById(lastId.Value);


            }
        }
        /* ========================= LISTAS ========================= */
        private void CargarListas()
        {
            // Cruce
            ddlCruce.Items.Clear();
            ddlCruce.Items.Add(new ListItem("", ""));
            ddlCruce.Items.Add(new ListItem("A1 - Cruce principal", "A1"));
            ddlCruce.Items.Add(new ListItem("A4 - Alterno/segundo cruce", "A4"));
            // Incoterm (ya en markup)
            // Empresa
            ddlEmpresa.Items.Clear();
            ddlEmpresa.Items.Add(new ListItem("", ""));
            ddlEmpresa.Items.Add(new ListItem("J&J Seafood", "J&J Seafood"));
            ddlEmpresa.Items.Add(new ListItem("Central", "Central"));
            // Producto
            ddlProducto.Items.Clear();
            ddlProducto.Items.Add(new ListItem("", ""));
            ddlProducto.Items.Add(new ListItem("TIBURÓN MARRAJO HGT", "TIBURÓN MARRAJO HGT"));
            ddlProducto.Items.Add(new ListItem("FILETE DE SALMON COHO", "FILETE DE SALMON COHO"));
            ddlProducto.Items.Add(new ListItem("FILETE DE SALMON SALAR", "FILETE DE SALMON SALAR"));
            ddlProducto.Items.Add(new ListItem("FILETE TILAPIA 47%NW", "FILETE TILAPIA 47%NW"));
            ddlProducto.Items.Add(new ListItem("FILETE TILAPIA 92%NW", "FILETE TILAPIA 92%NW"));
            ddlProducto.Items.Add(new ListItem("FILETE TILAPIA 100%NW", "FILETE TILAPIA 100%NW"));
            ddlProducto.Items.Add(new ListItem("FILETE BASA 100%NW", "FILETE BASA 100%NW"));
            // Marca
            ddlMarca.Items.Clear();
            ddlMarca.Items.Add(new ListItem("", ""));
            ddlMarca.Items.Add(new ListItem("AQUA CHILE", "AQUA CHILE"));
            ddlMarca.Items.Add(new ListItem("SIN MARCA", "SIN MARCA"));
            ddlMarca.Items.Add(new ListItem("CEPESMAR", "CEPEsMAR"));
            ddlMarca.Items.Add(new ListItem("REY DE MAR", "REY DE MAR"));
            ddlMarca.Items.Add(new ListItem("CONARTA", "CONARTA"));
            // Talla
            ddlTalla.Items.Clear();
            ddlTalla.Items.Add(new ListItem("", ""));
            ddlTalla.Items.Add(new ListItem("2/3", "2/3"));
            ddlTalla.Items.Add(new ListItem("3/4", "3/4"));
            ddlTalla.Items.Add(new ListItem("3/5", "3/5"));
            ddlTalla.Items.Add(new ListItem("4-23 KG", "4-23 KG"));
            ddlTalla.Items.Add(new ListItem("4/5", "4/5"));
            ddlTalla.Items.Add(new ListItem("5/7", "5/7"));
            ddlTalla.Items.Add(new ListItem("7/9", "7/9"));
            ddlTalla.Items.Add(new ListItem("8/10", "8/10"));
        }
        /* ========================= GRID / BÚSQUEDAS ========================= */
        private void BindGrid(string filtro = null)
        {
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(@" 
SELECT TOP (200) Id, FechaCompra, Contract, Container, Proveedor, ETA 
FROM hd.Carga 
WHERE (@filtro IS NULL OR 
 Contract LIKE '%'+@filtro+'%' OR 
 Container LIKE '%'+@filtro+'%' OR 
 Proveedor LIKE '%'+@filtro+'%') 
ORDER BY Id DESC;", con))
            {
                cmd.Parameters.Add("@filtro", SqlDbType.NVarChar, 200).Value =
                string.IsNullOrWhiteSpace(filtro) ? (object)DBNull.Value : filtro.Trim();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                gvCargas.DataSource = dt;
                gvCargas.DataBind();
            }
        }
        protected void gvCargas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCargas.PageIndex = e.NewPageIndex;
            BindGrid(txtFiltro.Text);
        }
        protected void gvCargas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(gvCargas.DataKeys[gvCargas.SelectedIndex].Value);
            LoadById(id);
        }
        protected void btnBuscar_Click(object sender, EventArgs e) => BindGrid(txtFiltro.Text);
        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            BindGrid();
        }
        /* ========================= CARGA / ID ACTUAL ========================= */
        private int? GetCurrentId() =>
        int.TryParse(hidId?.Value, out var id) ? id : (int?)null;
        private void SetCurrentId(int? id)
        {
            hidId.Value = id.HasValue ? id.Value.ToString() : string.Empty;
            btnActualizar.Enabled = btnEliminar.Enabled = id.HasValue;
        }
        private int? GetLastId()
        {
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand("SELECT TOP 1 Id FROM hd.Carga ORDER BY Id DESC;", con))
            {
                con.Open();
                var o = cmd.ExecuteScalar();
                return o == null || o == DBNull.Value ? (int?)null : Convert.ToInt32(o);
            }
        }
        private void LoadById(int id)
        {
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(@"SELECT TOP 1 * FROM hd.Carga WHERE Id=@Id;", con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "nf", "alert('No se encontró el ID.');", true);
                        return;
                    }
                    txtFechaCompra.Text = ToDateStr(r["FechaCompra"]);
                    SetDdlValue(ddlCruce, NullToEmpty(r["Cruce"]));
                    SetDdlValue(ddlIncoterm, NullToEmpty(r["Incoterm"]));
                    txtContract.Text = r["Contract"] as string;
                    txtContainer.Text = r["Container"] as string;
                    txtSello.Text = r["Sello"] as string;
                    txtInvoice.Text = r["Invoice"] as string;
                    txtPedimento.Text = r["Pedimento"] as string;
                    txtProveedor.Text = r["Proveedor"] as string;
                    txtOrigen.Text = r["Origen"] as string;
                    txtPuerto.Text = r["Puerto"] as string;
                    txtNaviera.Text = r["Naviera"] as string;
                    txtConexionesDemoras.Text = r["ConexionesDemoras"] as string;
                    txtETD.Text = ToDateStr(r["ETD"]);
                    txtETA.Text = ToDateStr(r["ETA"]);
                    txtFechaEstimDestino.Text = ToDateStr(r["FechaEstimDestino"]);
                    txtUltimoDiaLibre.Text = ToDateStr(r["UltimoDiaLibreDemoras"]);
                    txtEntregaVacio.Text = ToDateStr(r["EntregaVacio"]);
                    txtDiasDemoras.Text = ToIntStr(r["DiasGeneradosDemoras"]);
                    txtFacturaDemoras.Text = r["FacturaDemoras"] as string;
                    txtMontoUSDDemoras.Text = ToDecStr(r["MontoUSDDemoras"], 2);
                    txtTCDemoras.Text = ToDecStr(r["TCDemoras"], 4);
                    txtMontoDemorasMXN.Text = ToDecStr(r["MontoDemorasMXN"], 2);
                    txtFacturaComercializadora.Text = r["FacturaComercializadora"] as string;
                    txtFechaDespacho.Text = ToDateStr(r["FechaDespacho"]);
                    txtFechaPagoPedimento.Text = ToDateStr(r["FechaPagoPedimento"]);
                    txtPlanta.Text = ToIntStr(r["Planta"]);
                    txtLote.Text = r["Lote"] as string;
                    txtFechaProduccion.Text = ToDateStr(r["FechaProduccion"]);
                    txtFechaCaducidad.Text = ToDateStr(r["FechaCaducidad"]);
                    SetDdlValue(ddlEmpresa, NullToEmpty(r["Empresa"]));
                    txtCodigo.Text = r["Codigo"] as string;
                    SetDdlValue(ddlProducto, NullToEmpty(r["Producto"]));
                    SetDdlValue(ddlMarca, NullToEmpty(r["Marca"]));
                    SetDdlValue(ddlTalla, NullToEmpty(r["Talla"]));
                    txtKgs.Text = ToDecStr(r["Kgs"], 2);
                    txtCajas.Text = ToIntStr(r["Cajas"]);
                    SetCurrentId(id);
                }
            }
            BindArchivos();
        }
        /* ========================= GUARDAR (INSERT) ========================= */
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            try
            {
                // --- Lectura/parseo --- 
                DateTime fechaCompra = DateTime.Parse(txtFechaCompra.Text);
                string cruce = SafeStr(ddlCruce?.SelectedValue);
                string incoterm = SafeStr(ddlIncoterm?.SelectedValue);
                string contract = SafeStr(txtContract?.Text);
                string container = SafeStr(txtContainer?.Text);
                string sello = SafeStr(txtSello?.Text);
                string invoice = SafeStr(txtInvoice?.Text);
                string pedimento = SafeStr(txtPedimento?.Text);
                string proveedor = SafeStr(txtProveedor?.Text);
                string origen = SafeStr(txtOrigen?.Text);
                string puerto = SafeStr(txtPuerto?.Text);
                string naviera = SafeStr(txtNaviera?.Text);
                string conexiones = SafeStr(txtConexionesDemoras?.Text);
                DateTime? etd = ParseDate(txtETD?.Text);
                DateTime? eta = ParseDate(txtETA?.Text);
                DateTime? fechaEstimDestino = ParseDate(txtFechaEstimDestino?.Text);
                DateTime? ultimoDiaLibre = ParseDate(txtUltimoDiaLibre?.Text);
                DateTime? entregaVacio = ParseDate(txtEntregaVacio?.Text);
                int? diasGeneradosDemoras = ParseInt(txtDiasDemoras?.Text);
                string facturaDemoras = SafeStr(txtFacturaDemoras?.Text);
                decimal? montoUsdDemoras = ParseDecimal(txtMontoUSDDemoras?.Text, 2);
                decimal? tcDemoras = ParseDecimal(txtTCDemoras?.Text, 4);
                decimal? montoDemorasMxn = ParseDecimal(txtMontoDemorasMXN?.Text, 2);
                if ((!montoDemorasMxn.HasValue || montoDemorasMxn.Value == 0m) &&
                montoUsdDemoras.HasValue && tcDemoras.HasValue)
                    montoDemorasMxn = Math.Round(montoUsdDemoras.Value * tcDemoras.Value, 2);
                if (!diasGeneradosDemoras.HasValue && ultimoDiaLibre.HasValue && entregaVacio.HasValue)
                {
                    var diff = (entregaVacio.Value.Date - ultimoDiaLibre.Value.Date).Days;
                    diasGeneradosDemoras = diff > 0 ? diff : 0;
                }
                string facturaComercializadora = SafeStr(txtFacturaComercializadora?.Text);
                DateTime? fechaDespacho = ParseDate(txtFechaDespacho?.Text);
                DateTime? fechaPagoPedimento = ParseDate(txtFechaPagoPedimento?.Text);
                int? planta = ParseInt(txtPlanta?.Text);
                string lote = SafeStr(txtLote?.Text);
                DateTime? fprod = ParseDate(txtFechaProduccion?.Text);
                DateTime? fcad = ParseDate(txtFechaCaducidad?.Text);
                string empresa = SafeStr(ddlEmpresa?.SelectedValue);
                string codigo = SafeStr(txtCodigo?.Text);
                string producto = SafeStr(ddlProducto?.SelectedValue);
                string marca = SafeStr(ddlMarca?.SelectedValue);
                string talla = SafeStr(ddlTalla?.SelectedValue);
                decimal? kgs = ParseDecimal(txtKgs?.Text, 2);
                int? cajas = ParseInt(txtCajas?.Text);
                // Usuario/creador (opcional, desde Session) 
                object creadoPorObj = Session["userid"] ?? Session["agentid"] ?? (object)DBNull.Value;
                long? creadoPor = null;
                if (creadoPorObj is string s && long.TryParse(s, out var creadoPorId)) creadoPor = creadoPorId;
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand(@" 
INSERT INTO hd.Carga 
( 
 FechaCompra, Cruce, Incoterm, Contract, Container, Sello, Invoice, Pedimento, 
 Proveedor, Origen, Puerto, Naviera, ConexionesDemoras, 
 ETD, ETA, FechaEstimDestino, UltimoDiaLibreDemoras, EntregaVacio, DiasGeneradosDemoras, 
 FacturaDemoras, MontoUSDDemoras, TCDemoras, MontoDemorasMXN, 
 FacturaComercializadora, FechaDespacho, FechaPagoPedimento, Planta, 
 Lote, FechaProduccion, FechaCaducidad, Empresa, Codigo, 
 Producto, Marca, Talla, Kgs, Cajas, CreadoPor 
) 
VALUES 
( 
 @FechaCompra, @Cruce, @Incoterm, @Contract, @Container, @Sello, @Invoice, @Pedimento, 
 @Proveedor, @Origen, @Puerto, @Naviera, @ConexionesDemoras, 
 @ETD, @ETA, @FechaEstimDestino, @UltimoDiaLibreDemoras, @EntregaVacio, @DiasGeneradosDemoras, 
 @FacturaDemoras, @MontoUSDDemoras, @TCDemoras, @MontoDemorasMXN, 
 @FacturaComercializadora, @FechaDespacho, @FechaPagoPedimento, @Planta, 
 @Lote, @FechaProduccion, @FechaCaducidad, @Empresa, @Codigo, 
 @Producto, @Marca, @Talla, @Kgs, @Cajas, @CreadoPor 
); 
SELECT CAST(SCOPE_IDENTITY() AS int);", con))
                {
                    cmd.Parameters.Add("@FechaCompra", SqlDbType.Date).Value = fechaCompra;
                    AddNVar(cmd, "@Cruce", 10, cruce);
                    AddNVar(cmd, "@Incoterm", 10, incoterm);
                    AddNVar(cmd, "@Contract", 40, contract);
                    AddNVar(cmd, "@Container", 40, container);
                    AddNVar(cmd, "@Sello", 40, sello);
                    AddNVar(cmd, "@Invoice", 40, invoice);
                    AddNVar(cmd, "@Pedimento", 30, pedimento);
                    AddNVar(cmd, "@Proveedor", 200, proveedor);
                    AddNVar(cmd, "@Origen", 60, origen);
                    AddNVar(cmd, "@Puerto", 60, puerto);
                    AddNVar(cmd, "@Naviera", 60, naviera);
                    AddNVar(cmd, "@ConexionesDemoras", 40, conexiones);
                    AddDate(cmd, "@ETD", etd);
                    AddDate(cmd, "@ETA", eta);
                    AddDate(cmd, "@FechaEstimDestino", fechaEstimDestino);
                    AddDate(cmd, "@UltimoDiaLibreDemoras", ultimoDiaLibre);
                    AddDate(cmd, "@EntregaVacio", entregaVacio);
                    AddInt(cmd, "@DiasGeneradosDemoras", diasGeneradosDemoras);
                    AddNVar(cmd, "@FacturaDemoras", 40, facturaDemoras);
                    AddDec(cmd, "@MontoUSDDemoras", 18, 2, montoUsdDemoras);
                    AddDec(cmd, "@TCDemoras", 18, 4, tcDemoras);
                    AddDec(cmd, "@MontoDemorasMXN", 18, 2, montoDemorasMxn);
                    AddNVar(cmd, "@FacturaComercializadora", 40, facturaComercializadora);
                    AddDate(cmd, "@FechaDespacho", fechaDespacho);
                    AddDate(cmd, "@FechaPagoPedimento", fechaPagoPedimento);
                    AddInt(cmd, "@Planta", planta);
                    AddNVar(cmd, "@Lote", 60, lote);
                    AddDate(cmd, "@FechaProduccion", fprod);
                    AddDate(cmd, "@FechaCaducidad", fcad);
                    AddNVar(cmd, "@Empresa", 100, empresa);
                    AddNVar(cmd, "@Codigo", 40, codigo);
                    AddNVar(cmd, "@Producto", 200, producto);
                    AddNVar(cmd, "@Marca", 120, marca);
                    AddNVar(cmd, "@Talla", 40, talla);
                    AddDec(cmd, "@Kgs", 18, 2, kgs);
                    AddInt(cmd, "@Cajas", cajas);
                    if (creadoPor.HasValue) cmd.Parameters.Add("@CreadoPor", SqlDbType.BigInt).Value = creadoPor.Value;
                    else cmd.Parameters.Add("@CreadoPor", SqlDbType.BigInt).Value = DBNull.Value;
                    con.Open();
                    var newId = (int)cmd.ExecuteScalar();
                    LoadById(newId);
                    BindGrid(txtFiltro.Text);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    $"alert('Registro guardado. ID: {newId}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err",
                $"alert('Error al guardar: {HttpUtility.JavaScriptStringEncode(ex.Message)}');", true);
            }
        }
        /* ========================= ACTUALIZAR (UPDATE) ========================= */
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            var curId = GetCurrentId();
            if (!curId.HasValue)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "needid", "alert('No hay ID cargado para actualizar.');", true);
                return;
            }
            try
            {
                // (Mismo parseo que en Guardar) 
                DateTime fechaCompra = DateTime.Parse(txtFechaCompra.Text);
                string cruce = SafeStr(ddlCruce?.SelectedValue);
                string incoterm = SafeStr(ddlIncoterm?.SelectedValue);
                string contract = SafeStr(txtContract?.Text);
                string container = SafeStr(txtContainer?.Text);
                string sello = SafeStr(txtSello?.Text);
                string invoice = SafeStr(txtInvoice?.Text);
                string pedimento = SafeStr(txtPedimento?.Text);
                string proveedor = SafeStr(txtProveedor?.Text);
                string origen = SafeStr(txtOrigen?.Text);
                string puerto = SafeStr(txtPuerto?.Text);
                string naviera = SafeStr(txtNaviera?.Text);
                string conexiones = SafeStr(txtConexionesDemoras?.Text);
                DateTime? etd = ParseDate(txtETD?.Text);
                DateTime? eta = ParseDate(txtETA?.Text);
                DateTime? fechaEstimDestino = ParseDate(txtFechaEstimDestino?.Text);
                DateTime? ultimoDiaLibre = ParseDate(txtUltimoDiaLibre?.Text);
                DateTime? entregaVacio = ParseDate(txtEntregaVacio?.Text);
                int? diasGeneradosDemoras = ParseInt(txtDiasDemoras?.Text);
                string facturaDemoras = SafeStr(txtFacturaDemoras?.Text);
                decimal? montoUsdDemoras = ParseDecimal(txtMontoUSDDemoras?.Text, 2);
                decimal? tcDemoras = ParseDecimal(txtTCDemoras?.Text, 4);
                decimal? montoDemorasMxn = ParseDecimal(txtMontoDemorasMXN?.Text, 2);
                if ((!montoDemorasMxn.HasValue || montoDemorasMxn.Value == 0m) &&
                montoUsdDemoras.HasValue && tcDemoras.HasValue)
                    montoDemorasMxn = Math.Round(montoUsdDemoras.Value * tcDemoras.Value, 2);
                if (!diasGeneradosDemoras.HasValue && ultimoDiaLibre.HasValue && entregaVacio.HasValue)
                {
                    var diff = (entregaVacio.Value.Date - ultimoDiaLibre.Value.Date).Days;
                    diasGeneradosDemoras = diff > 0 ? diff : 0;
                }
                string facturaComercializadora = SafeStr(txtFacturaComercializadora?.Text);
                DateTime? fechaDespacho = ParseDate(txtFechaDespacho?.Text);
                DateTime? fechaPagoPedimento = ParseDate(txtFechaPagoPedimento?.Text);
                int? planta = ParseInt(txtPlanta?.Text);
                string lote = SafeStr(txtLote?.Text);
                DateTime? fprod = ParseDate(txtFechaProduccion?.Text);
                DateTime? fcad = ParseDate(txtFechaCaducidad?.Text);
                string empresa = SafeStr(ddlEmpresa?.SelectedValue);
                string codigo = SafeStr(txtCodigo?.Text);
                string producto = SafeStr(ddlProducto?.SelectedValue);
                string marca = SafeStr(ddlMarca?.SelectedValue);
                string talla = SafeStr(ddlTalla?.SelectedValue);
                decimal? kgs = ParseDecimal(txtKgs?.Text, 2);
                int? cajas = ParseInt(txtCajas?.Text);
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand(@" 
UPDATE hd.Carga SET 
 FechaCompra=@FechaCompra, Cruce=@Cruce, Incoterm=@Incoterm, Contract=@Contract, Container=@Container, 
 Sello=@Sello, Invoice=@Invoice, Pedimento=@Pedimento, 
 Proveedor=@Proveedor, Origen=@Origen, Puerto=@Puerto, Naviera=@Naviera, ConexionesDemoras=@ConexionesDemoras, 
 ETD=@ETD, ETA=@ETA, FechaEstimDestino=@FechaEstimDestino, UltimoDiaLibreDemoras=@UltimoDiaLibreDemoras, 
 EntregaVacio=@EntregaVacio, DiasGeneradosDemoras=@DiasGeneradosDemoras, 
 FacturaDemoras=@FacturaDemoras, MontoUSDDemoras=@MontoUSDDemoras, TCDemoras=@TCDemoras, MontoDemorasMXN=@MontoDemorasMXN, 
 FacturaComercializadora=@FacturaComercializadora, FechaDespacho=@FechaDespacho, FechaPagoPedimento=@FechaPagoPedimento, Planta=@Planta, 
 Lote=@Lote, FechaProduccion=@FechaProduccion, FechaCaducidad=@FechaCaducidad, Empresa=@Empresa, Codigo=@Codigo, 
 Producto=@Producto, Marca=@Marca, Talla=@Talla, Kgs=@Kgs, Cajas=@Cajas 
WHERE Id=@Id;", con))
                {
                    cmd.Parameters.Add("@FechaCompra", SqlDbType.Date).Value = fechaCompra;
                    AddNVar(cmd, "@Cruce", 10, cruce);
                    AddNVar(cmd, "@Incoterm", 10, incoterm);
                    AddNVar(cmd, "@Contract", 40, contract);
                    AddNVar(cmd, "@Container", 40, container);
                    AddNVar(cmd, "@Sello", 40, sello);
                    AddNVar(cmd, "@Invoice", 40, invoice);
                    AddNVar(cmd, "@Pedimento", 30, pedimento);
                    AddNVar(cmd, "@Proveedor", 200, proveedor);
                    AddNVar(cmd, "@Origen", 60, origen);
                    AddNVar(cmd, "@Puerto", 60, puerto);
                    AddNVar(cmd, "@Naviera", 60, naviera);
                    AddNVar(cmd, "@ConexionesDemoras", 40, conexiones);
                    AddDate(cmd, "@ETD", etd);
                    AddDate(cmd, "@ETA", eta);
                    AddDate(cmd, "@FechaEstimDestino", fechaEstimDestino);
                    AddDate(cmd, "@UltimoDiaLibreDemoras", ultimoDiaLibre);
                    AddDate(cmd, "@EntregaVacio", entregaVacio);
                    AddInt(cmd, "@DiasGeneradosDemoras", diasGeneradosDemoras);
                    AddNVar(cmd, "@FacturaDemoras", 40, facturaDemoras);
                    AddDec(cmd, "@MontoUSDDemoras", 18, 2, montoUsdDemoras);
                    AddDec(cmd, "@TCDemoras", 18, 4, tcDemoras);
                    AddDec(cmd, "@MontoDemorasMXN", 18, 2, montoDemorasMxn);
                    AddNVar(cmd, "@FacturaComercializadora", 40, facturaComercializadora);
                    AddDate(cmd, "@FechaDespacho", fechaDespacho);
                    AddDate(cmd, "@FechaPagoPedimento", fechaPagoPedimento);
                    AddInt(cmd, "@Planta", planta);
                    AddNVar(cmd, "@Lote", 60, lote);
                    AddDate(cmd, "@FechaProduccion", fprod);
                    AddDate(cmd, "@FechaCaducidad", fcad);
                    AddNVar(cmd, "@Empresa", 100, empresa);
                    AddNVar(cmd, "@Codigo", 40, codigo);
                    AddNVar(cmd, "@Producto", 200, producto); 
                    AddNVar(cmd, "@Marca", 120, marca);
                    AddNVar(cmd, "@Talla", 40, talla);
                    AddDec(cmd, "@Kgs", 18, 2, kgs);
                    AddInt(cmd, "@Cajas", cajas);
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = curId.Value;
                    con.Open();
                    var rows = cmd.ExecuteNonQuery();
                    BindGrid(txtFiltro.Text);
                    ScriptManager.RegisterStartupScript(this, GetType(), "okupd",
                    $"alert('Registro {curId.Value} actualizado. Filas: {rows}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errupd",
                $"alert('Error al actualizar: {HttpUtility.JavaScriptStringEncode(ex.Message)}');", true);
            }
        }
        /* ========================= ELIMINAR (DELETE) ========================= */
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            var curId = GetCurrentId();
            if (!curId.HasValue)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "needid2", "alert('No hay ID cargado para eliminar.');", true);
                return;
            }
            try
            {
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand("DELETE FROM hd.Carga WHERE Id=@Id;", con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = curId.Value;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                LimpiarFormulario();
                SetCurrentId(null);
                BindGrid(txtFiltro.Text);
                ScriptManager.RegisterStartupScript(this, GetType(), "okdel",
                "alert('Registro eliminado.');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errdel",
                $"alert('Error al eliminar: {HttpUtility.JavaScriptStringEncode(ex.Message)}');", true);
            }
            BindArchivos();
        }
        /* ========================= HELPERS ========================= */
        private static string SafeStr(string s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        private static DateTime? ParseDate(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return null;
            return DateTime.TryParse(val, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)
            || DateTime.TryParse(val, CultureInfo.GetCultureInfo("es-MX"), DateTimeStyles.None, out d)
            ? d : (DateTime?)null;
        }
        private static decimal? ParseDecimal(string val, int roundTo)
        {
            if (string.IsNullOrWhiteSpace(val)) return null;
            if (decimal.TryParse(val, NumberStyles.Number, CultureInfo.InvariantCulture, out var v)
            || decimal.TryParse(val, NumberStyles.Number, CultureInfo.GetCultureInfo("es-MX"), out v))
                return Math.Round(v, roundTo);
            var swapped = val.Contains(",") ? val.Replace(".", "").Replace(",", ".") : val;
            if (decimal.TryParse(swapped, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                return Math.Round(v, roundTo);
            return null;
        }
        private static int? ParseInt(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return null;
            return int.TryParse(val, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : (int?)null;
        }
        private static void AddNVar(SqlCommand cmd, string name, int size, string val)
        {
            cmd.Parameters.Add(name, SqlDbType.NVarChar, size).Value =
            string.IsNullOrWhiteSpace(val) ? (object)DBNull.Value : val.Trim();
        }
        private static void AddDate(SqlCommand cmd, string name, DateTime? val)
        {
            cmd.Parameters.Add(name, SqlDbType.Date).Value = val.HasValue ? (object)val.Value.Date : DBNull.Value;
        }
        private static void AddInt(SqlCommand cmd, string name, int? val)
        {
            cmd.Parameters.Add(name, SqlDbType.Int).Value = val.HasValue ? (object)val.Value : DBNull.Value;
        }
        private static void AddDec(SqlCommand cmd, string name, byte precision, byte scale, decimal? val)
        {
            var p = cmd.Parameters.Add(name, SqlDbType.Decimal);
            p.Precision = precision;
            p.Scale = scale;
            p.Value = val.HasValue ? (object)val.Value : DBNull.Value;
        }
        private static string ToDateStr(object v) =>
        v == DBNull.Value ? "" : ((DateTime)v).ToString("yyyy-MM-dd");
        private static string ToIntStr(object v) =>
        v == DBNull.Value ? "" : Convert.ToInt32(v).ToString(CultureInfo.InvariantCulture);
        private static string ToDecStr(object v, int decs) =>
        v == DBNull.Value ? "" : Math.Round(Convert.ToDecimal(v), decs).ToString($"F{decs}", CultureInfo.InvariantCulture);
        private static string NullToEmpty(object v) =>
        v == DBNull.Value ? "" : Convert.ToString(v);
        private static void SetDdlValue(DropDownList ddl, string value)
        {
            if (string.IsNullOrEmpty(value)) { ddl.ClearSelection(); return; }
            var item = ddl.Items.FindByValue(value);
            if (item != null) { ddl.ClearSelection(); item.Selected = true; }
            else ddl.ClearSelection();
        }
        private void LimpiarFormulario()
        {
            txtFechaCompra.Text = "";
            ddlIncoterm.ClearSelection();
            ddlCruce.ClearSelection();
            txtContract.Text = txtContainer.Text = txtSello.Text = txtInvoice.Text = txtPedimento.Text = "";
            txtProveedor.Text = txtOrigen.Text = txtPuerto.Text = txtNaviera.Text = txtConexionesDemoras.Text = "";
            txtETD.Text = txtETA.Text = txtFechaEstimDestino.Text = "";
            txtUltimoDiaLibre.Text = txtEntregaVacio.Text = txtDiasDemoras.Text = "";
            txtFacturaDemoras.Text = txtMontoUSDDemoras.Text = txtTCDemoras.Text = txtMontoDemorasMXN.Text = "";
            txtFacturaComercializadora.Text = txtFechaDespacho.Text = txtFechaPagoPedimento.Text = "";
            txtPlanta.Text = "";
            txtLote.Text = "";
            txtFechaProduccion.Text = "";
            txtFechaCaducidad.Text = "";
            ddlEmpresa.ClearSelection();
            txtCodigo.Text = "";
            ddlProducto.ClearSelection();
            ddlMarca.ClearSelection();
            ddlTalla.ClearSelection();
            txtKgs.Text = txtCajas.Text = "";
            BindArchivos();
        }


        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            CargarDescripcionProducto();
        }

        private void CargarDescripcionProducto()
        {
            string codigo = txtCodigo.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                txtDescripcionProducto.Text = "";
                return;
            }



            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(
                @"SELECT Producto, Talla 
          FROM hd.Catalogo 
          WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", codigo);
                con.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        string prod = rd["Producto"]?.ToString() ?? "";
                        string talla = rd["Talla"]?.ToString() ?? "";
                        txtDescripcionProducto.Text = $"{prod} {talla}".Trim();
                    }
                    else
                    {
                        txtDescripcionProducto.Text = "No encontrado en catálogo";
                    }
                }
            }


        }

        private void CargarListaProductos()
        {
            ddlProducto.Items.Clear();
            // 1) Primer elemento vacío
            ddlProducto.Items.Add(new ListItem(string.Empty, string.Empty));

            // 2) Traer Productos únicos desde hd.Catalogo
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(@"
        SELECT DISTINCT Producto
        FROM hd.Catalogo
        WHERE Producto IS NOT NULL AND LTRIM(RTRIM(Producto)) <> ''
        ORDER BY Producto;", con))
            {
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var prod = rd["Producto"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(prod))
                        {
                            // Evita duplicados por si acaso
                            if (ddlProducto.Items.FindByValue(prod) == null)
                                ddlProducto.Items.Add(new ListItem(prod, prod));
                        }
                    }
                }
            }
        }


        // Folder físico donde guardamos (por carga)
        private string GetUploadFolder(int cargaId)
        {
            return Server.MapPath($"~/Uploads/Cargas/{cargaId}");
        }

        // Ruta virtual (URL) para abrir en el navegador
        private string GetUploadVirtual(int cargaId)
        {
            return ResolveUrl($"~/Uploads/Cargas/{cargaId}/");
        }

        // Enlaza el grid de archivos para el ID actual
        private void BindArchivos()
        {
            var curId = GetCurrentId();
            lblIdActual.Text = curId.HasValue ? $"ID actual: {curId.Value}" : "Sin ID seleccionado";

            var dt = new System.Data.DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("SizeKB", typeof(long));
            dt.Columns.Add("Fecha", typeof(System.DateTime));
            dt.Columns.Add("Url", typeof(string));

            if (!curId.HasValue)
            {
                gvArchivos.DataSource = dt;
                gvArchivos.DataBind();
                btnSubirArchivo.Enabled = fuArchivo.Enabled = false;
                lblUploadMsg.Text = "Selecciona un registro (ID) para habilitar la carga.";
                return;
            }

            btnSubirArchivo.Enabled = fuArchivo.Enabled = true;
            lblUploadMsg.Text = "";

            var dir = GetUploadFolder(curId.Value);
            var vdir = GetUploadVirtual(curId.Value);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (var path in Directory.GetFiles(dir))
            {
                var fi = new FileInfo(path);
                var row = dt.NewRow();
                row["Name"] = fi.Name;
                row["SizeKB"] = Math.Max(1, fi.Length / 1024);
                row["Fecha"] = fi.LastWriteTime;
                // Importante: escapamos el nombre para URL
                row["Url"] = vdir + Uri.EscapeDataString(fi.Name);
                dt.Rows.Add(row);
            }

            gvArchivos.DataSource = dt;
            gvArchivos.DataBind();
        }

        // Subir archivo
        protected void btnSubirArchivo_Click(object sender, EventArgs e)
        {
            var curId = GetCurrentId();
            if (!curId.HasValue)
            {
                lblUploadMsg.Text = "No hay ID seleccionado.";
                return;
            }

            if (!fuArchivo.HasFile)
            {
                lblUploadMsg.Text = "Selecciona un archivo.";
                return;
            }

            try
            {
                var dir = GetUploadFolder(curId.Value);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Nombre seguro (evita rutas)
                var originalName = Path.GetFileName(fuArchivo.FileName);

                // Si ya existe, versionamos con timestamp
                var savePath = Path.Combine(dir, originalName);
                if (File.Exists(savePath))
                {
                    string baseName = Path.GetFileNameWithoutExtension(originalName);
                    string ext = Path.GetExtension(originalName);
                    string stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    savePath = Path.Combine(dir, $"{baseName}_{stamp}{ext}");
                }

                fuArchivo.SaveAs(savePath);
                lblUploadMsg.Text = "Archivo subido correctamente.";
                BindArchivos();   // Refresca grid
            }
            catch (Exception ex)
            {
                lblUploadMsg.Text = "Error al subir: " + ex.Message;
            }
        }

        // Agrega doble‑clic por fila para abrir el archivo
        protected void gvArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var url = DataBinder.Eval(e.Row.DataItem, "Url") as string;
                e.Row.Attributes["ondblclick"] = $"openFile('{url}')";
                e.Row.Attributes["class"] = (e.Row.Attributes["class"] + " data-row").Trim();
            }
        }


    }
}