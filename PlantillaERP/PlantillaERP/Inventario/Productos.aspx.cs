using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maqueta.Inventario
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private readonly string _strcon = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }

            // Enter en el buscador dispara el botón Buscar
            btnBuscar.UseSubmitBehavior = false;
            txtBuscar.Attributes["onkeypress"] = $"if(event.keyCode==13){{document.getElementById('{btnBuscar.ClientID}').click(); return false;}}";
        }

        // Guarda/recupera el término de búsqueda
        private string SearchTerm
        {
            get => (ViewState["SearchTerm"] as string) ?? string.Empty;
            set => ViewState["SearchTerm"] = value;
        }

        private void BindGrid(string sortExpression = null, string sortDirection = null)
        {
            var dt = new DataTable();

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

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                sql += @"
          AND (
                (Producto IS NOT NULL AND Producto LIKE @q)
             OR (Talla IS NOT NULL AND Talla LIKE @q)
             OR ((Producto IS NOT NULL AND Talla IS NOT NULL) AND (Producto + ' ' + Talla) LIKE @q)
             OR (Id LIKE @q)                     -- <=== NUEVO: búsqueda por Id
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
            using (var da = new SqlDataAdapter(cmd))
            {
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                    cmd.Parameters.Add(new SqlParameter("@q", SqlDbType.NVarChar, 200) { Value = $"%{SearchTerm.Trim()}%" });

                da.Fill(dt);
            }

            gvProductos.DataSource = dt;
            gvProductos.DataBind();

            if (gvProductos.HeaderRow != null)
            {
                gvProductos.UseAccessibleHeader = true;
                gvProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        /* =======================
           Acciones de Buscador
           ======================= */
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            SearchTerm = txtBuscar.Text;
            gvProductos.PageIndex = 0;
            var se = ViewState["SortExpression"] as string;
            var sd = ViewState["SortDirection"] as string ?? "ASC";
            BindGrid(se, sd);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            SearchTerm = string.Empty;
            gvProductos.PageIndex = 0;
            var se = ViewState["SortExpression"] as string;
            var sd = ViewState["SortDirection"] as string ?? "ASC";
            BindGrid(se, sd);


            lblMsg.Text = "";
            lblMsg.CssClass = "text-success d-block mb-2";
        }

        /* =======================
           Paginación / Orden
           ======================= */
        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            var se = ViewState["SortExpression"] as string;
            var sd = ViewState["SortDirection"] as string ?? "ASC";
            BindGrid(se, sd);
        }

        protected void gvProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string currentExpr = ViewState["SortExpression"] as string;
            string currentDir = ViewState["SortDirection"] as string ?? "ASC";
            string newDir = (currentExpr == e.SortExpression && currentDir == "ASC") ? "DESC" : "ASC";

            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] = newDir;

            BindGrid(e.SortExpression, newDir);
        }

        /* =======================
           CRUD
           ======================= */

        // Cargar datos al formulario (desde el grid)
        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Cargar" || e.CommandName == "Eliminar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                var keys = gvProductos.DataKeys[rowIndex];

                string id = (keys["Id"] ?? string.Empty).ToString();
                string producto = (keys["Producto"] ?? string.Empty).ToString();
                string talla = (keys["Talla"] ?? string.Empty).ToString();

                if (e.CommandName == "Cargar")
                {
                    hfId.Value = id;              // Id original (clave)
                    txtId.Text = id;              // Mostrarlo
                    txtId.Enabled = false;        // No permitir cambiar PK en edición
                    txtProducto.Text = producto;
                    txtTalla.Text = talla;

                    btnGuardar.Text = "Actualizar";
                    lblMsg.Text = "Editando producto seleccionado.";
                    lblMsg.CssClass = "text-primary d-block mb-2";
                }
                else if (e.CommandName == "Eliminar")
                {
                    Eliminar(id);
                }
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string id = (txtId.Text ?? string.Empty).Trim();
            string producto = (txtProducto.Text ?? string.Empty).Trim();
            string talla = (txtTalla.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                lblMsg.Text = "El campo Id es obligatorio.";
                lblMsg.CssClass = "text-danger d-block mb-2";
                return;
            }
            if (string.IsNullOrWhiteSpace(producto))
            {
                lblMsg.Text = "El campo Producto es obligatorio.";
                lblMsg.CssClass = "text-danger d-block mb-2";
                return;
            }

            if (string.IsNullOrEmpty(hfId.Value))
                Insertar(id, producto, talla);
            else
                Actualizar(hfId.Value, producto, talla); // usamos el Id original de hfId
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

            lblMsg.Text = "";
            lblMsg.CssClass = "text-success d-block mb-2";
        }


        private void Insertar(string id, string producto, string talla)
        {
            try
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
                  

                // After success:
                ScriptManager.RegisterStartupScript(this, GetType(), "opOk",
                    "alert('Se ha Guardado Nuevo Producto');", true);


                LimpiarFormulario();
                Rebind();
            }

            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // PK/Unique constraint
            {

                // After error (optional; include ex.Message if desired):
                // NOTE: avoid leaking sensitive details in production
                ScriptManager.RegisterStartupScript(this, GetType(), "opErr",
                    "alert('El Id ya existe. Usa un Id diferente');", true);
                 
            }

        }

        private void Actualizar(string id, string producto, string talla)
        {
            try
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

                lblMsg.Text = "Producto actualizado.";
                lblMsg.CssClass = "text-success d-block mb-2";
                LimpiarFormulario();
                Rebind();
            }

            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // PK/Unique constraint
            {
                lblMsg.Text = "El Id ya existe. Usa un Id diferente.";
                lblMsg.CssClass = "text-danger d-block mb-2";
            }

        }

        private void Eliminar(string id)
        {
            try
            {
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand(
                    "DELETE FROM hd.Catalogo WHERE Id=@Id;", con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = id;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.Text = "Producto eliminado.";
                lblMsg.CssClass = "text-success d-block mb-2";
                LimpiarFormulario();
                Rebind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "No se pudo eliminar: " + ex.Message;
                lblMsg.CssClass = "text-danger d-block mb-2";
            }
        }
        private void Rebind()
        {
            var se = ViewState["SortExpression"] as string;
            var sd = ViewState["SortDirection"] as string ?? "ASC";
            BindGrid(se, sd);
        }


        private void LimpiarFormulario()
        {
            hfId.Value = string.Empty;
            txtId.Text = string.Empty;
            txtId.Enabled = true;   // volver a habilitar para nuevas altas
            txtProducto.Text = string.Empty;
            txtTalla.Text = string.Empty;
            btnGuardar.Text = "Guardar";

        }

    }
}