using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HelpDesk
{
    public partial class allUsers : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridViewUsers.DataBind(); // poblar al inicio
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GridViewUsers.PageIndex = 0;
            GridViewUsers.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            GridViewUsers.PageIndex = 0;
            GridViewUsers.DataBind(); // poblar al limpiar
        }

        protected void GridViewUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewUsers.PageIndex = e.NewPageIndex;
            GridViewUsers.DataBind();
        }

        protected void GridViewUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var drv = (DataRowView)e.Row.DataItem;

            string usuarioId = drv["UsuarioId"]?.ToString() ?? "";
            string nombre = drv["Nombre"]?.ToString() ?? "";
            string apPat = drv["ApPaterno"]?.ToString() ?? "";
            string apMat = drv["ApMaterno"]?.ToString() ?? "";
            string correo = drv["Correo"]?.ToString() ?? "";
            string empresaId = drv["EmpresaId"]?.ToString() ?? "";
            string puestoId = drv["PuestoId"]?.ToString() ?? "";
            string slaId = drv["SLAId"]?.ToString() ?? "";
            string activo = (drv["Activo"] != DBNull.Value && Convert.ToBoolean(drv["Activo"])) ? "True" : "False";

            // atributos data-* para JS
            e.Row.Attributes["class"] = (e.Row.Attributes["class"] + " row-click").Trim();
            e.Row.Attributes["data-usuarioid"] = usuarioId;
            e.Row.Attributes["data-nombre"] = nombre;
            e.Row.Attributes["data-appaterno"] = apPat;
            e.Row.Attributes["data-apmaterno"] = apMat;
            e.Row.Attributes["data-correo"] = correo;
            e.Row.Attributes["data-empresaid"] = empresaId;
            e.Row.Attributes["data-puestoid"] = puestoId;
            e.Row.Attributes["data-slaid"] = slaId;
            e.Row.Attributes["data-activo"] = activo;

            e.Row.Attributes["onclick"] = "rowClick(this)";
        }

        protected void modalBtnSave_Click(object sender, EventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(modalHiddenUsuarioId.Value)) return;
            if (!int.TryParse(modalHiddenUsuarioId.Value, out int usuarioId)) return;

            string nombre = modalTxtNombre.Text?.Trim() ?? "";
            string apPat = modalTxtApPaterno.Text?.Trim() ?? "";
            string apMat = modalTxtApMaterno.Text?.Trim() ?? "";
            string correo = modalTxtCorreo.Text?.Trim() ?? ""; // solo lectura (pero lo mandamos igual)

            bool activo = Request.Form["modalChkActivo"] == "on";


            int? empresaId = null;
            if (!string.IsNullOrWhiteSpace(modalDdlEmpresa.SelectedValue) && int.TryParse(modalDdlEmpresa.SelectedValue, out int emId))
                empresaId = emId;

            int? puestoId = null;
            if (!string.IsNullOrWhiteSpace(modalDdlPuesto.SelectedValue) && int.TryParse(modalDdlPuesto.SelectedValue, out int puId))
                puestoId = puId;

            int? slaId = null;
            if (!string.IsNullOrWhiteSpace(modalDdlSLA.SelectedValue) && int.TryParse(modalDdlSLA.SelectedValue, out int slId))
                slaId = slId;

            using (var cn = new SqlConnection(cs))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE [hd].[Usuario]
SET  Nombre        = @Nombre,
     ApPaterno     = @ApPaterno,
     ApMaterno     = @ApMaterno, 
     Empresa       = @Empresa,
     Puesto        = @Puesto,
     SLA           = @SLA,
     Activo        = @Activo,
     ActualizadoEn = SYSUTCDATETIME()
WHERE UsuarioId = @UsuarioId;";

                cmd.Parameters.AddWithValue("@Nombre", (object)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ApPaterno", (object)apPat ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ApMaterno", (object)apMat ?? DBNull.Value);
             
                // manejar NULL en FK
                cmd.Parameters.Add("@Empresa", SqlDbType.Int).Value = (object)empresaId ?? DBNull.Value;
                cmd.Parameters.Add("@Puesto", SqlDbType.Int).Value = (object)puestoId ?? DBNull.Value;
                cmd.Parameters.Add("@SLA", SqlDbType.Int).Value = (object)slaId ?? DBNull.Value;

                cmd.Parameters.AddWithValue("@Activo", activo);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            // Rebind de la tabla
            GridViewUsers.DataBind();

            // Cierra el modal en el cliente
            ScriptManager.RegisterStartupScript(this, GetType(), "hideUserModal", "$('#userModal').modal('hide');", true);


            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "restoreActivo",
                "document.getElementById('modalChkActivo').checked = " + (activo ? "true" : "false") + ";",
                true
            );

        }
    }
}