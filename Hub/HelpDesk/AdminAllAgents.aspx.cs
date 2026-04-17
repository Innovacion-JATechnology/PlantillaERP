using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HelpDesk
{
    public partial class AdminAllAgents : AdminOnlyPage
    {
        string cs = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridViewAgents.DataBind(); // poblar al inicio
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GridViewAgents.PageIndex = 0;
            GridViewAgents.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            GridViewAgents.PageIndex = 0;
            GridViewAgents.DataBind(); // poblar al limpiar
        }

        protected void GridViewAgents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAgents.PageIndex = e.NewPageIndex;
            GridViewAgents.DataBind();
        }

        protected void GridViewAgents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var drv = (DataRowView)e.Row.DataItem;

            string agenteId = drv["AgenteId"]?.ToString() ?? "";
            string nombre = drv["nombre"]?.ToString() ?? "";
            string nivel = drv["nivel"]?.ToString() ?? "";
            string tickets = drv["TicketsAbiertos"]?.ToString() ?? "0";
            string telefono = drv["telefono"]?.ToString() ?? "";
            string habilidades = drv["habilidades"]?.ToString() ?? "";
            string estatus = drv["Estatus"]?.ToString() ?? "1";

            // atributos data-* para JS
            e.Row.Attributes["class"] = (e.Row.Attributes["class"] + " row-click").Trim();
            e.Row.Attributes["data-agenteid"] = agenteId;
            e.Row.Attributes["data-nombre"] = nombre;
            e.Row.Attributes["data-nivel"] = nivel;
            e.Row.Attributes["data-tickets"] = tickets;
            e.Row.Attributes["data-telefono"] = telefono;
            e.Row.Attributes["data-habilidades"] = habilidades;
            e.Row.Attributes["data-estatus"] = estatus;

            e.Row.Attributes["onclick"] = "rowClick(this)";
        }

        protected void modalBtnSave_Click(object sender, EventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(modalHiddenAgenteId.Value)) return;
            if (!int.TryParse(modalHiddenAgenteId.Value, out int agenteId)) return;

            string nombre = modalTxtNombre.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(nombre)) return; // nombre es requerido

            int nivel = 0;
            if (!int.TryParse(modalTxtNivel.Text, out nivel)) return; // nivel es requerido

            string telefono = modalTxtTelefono.Text?.Trim() ?? "";
            string habilidades = modalTxtHabilidades.Text?.Trim() ?? "";

            int estatus = 1;
            if (!int.TryParse(modalDdlEstatus.SelectedValue, out estatus)) estatus = 1;

            using (var cn = new SqlConnection(cs))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE [hd].[Agente]
SET Nombre = @Nombre,
    Nivel = @Nivel,
    Telefono = @Telefono,
    Habilidades = @Habilidades,
    Estatus = @Estatus
WHERE AgenteId = @AgenteId;";

                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Nivel", nivel);
                cmd.Parameters.AddWithValue("@Telefono", (object)telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Habilidades", (object)habilidades ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estatus", estatus);
                cmd.Parameters.AddWithValue("@AgenteId", agenteId);

                cn.Open();
                cmd.ExecuteNonQuery();
            }

            // Rebind de la tabla - Limpia caché
            SqlDataSourceAgents.DataBind();
            GridViewAgents.DataBind();

            // Cierra el modal en el cliente
            ScriptManager.RegisterStartupScript(this, GetType(), "hideAgenteModal", "$('#agenteModal').modal('hide');", true);
        }

        protected void btnRecalculate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var cn = new SqlConnection(cs))
                using (var cmd = cn.CreateCommand())
                {
                    // Actualiza tAbiertos con el count de tickets asignados a cada agente
                    // Revisa todos los tickets abiertos/activos (Estatus != 7 y != 9, es decir no cerrados ni cancelados)
                    cmd.CommandText = @"
UPDATE [hd].[Agente]
SET tAbiertos = (
    SELECT COUNT(*)
    FROM [hd].[Ticket]
    WHERE [hd].[Ticket].AgenteId = [hd].[Agente].AgenteId
    AND [hd].[Ticket].Estatus IN (1, 2, 3, 4, 5, 8)
);";

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Rebind de la tabla para mostrar los cambios
                SqlDataSourceAgents.DataBind();
                GridViewAgents.DataBind();

                // Mostrar mensaje de éxito
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "recalculateSuccess",
                    "alert('Tickets abiertos recalculados exitosamente');",
                    true
                );
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "recalculateError",
                    "alert('Error al recalcular: " + ex.Message.Replace("'", "\\'") + "');",
                    true
                );
            }
        }
    }
}