using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace HelpDesk
{
    public partial class ActulizaTablas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Poblar todas las tablas al inicio
                gvEmpresas.DataBind();
                gvPuestos.DataBind();
                gvSla.DataBind();
            }
        }

        /* =========================================================
           EMPRESAS
           ========================================================= */

        protected void btnAgregarEmpresa_Click(object sender, EventArgs e)
        {
            ClearMessages();
            var nombre = (txtNuevaEmpresa.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                ShowError(lblMsgEmpresas, "El nombre de la empresa es obligatorio.");
                return;
            }

            try
            {
                SqlDataSourceEmpresa.InsertParameters["Nombre"].DefaultValue = nombre;
                SqlDataSourceEmpresa.Insert();

                txtNuevaEmpresa.Text = string.Empty;
                ShowSuccess(lblMsgEmpresas, "Empresa agregada correctamente.");
            }
            catch (Exception ex)
            {
                HandleSqlException(ex, lblMsgEmpresas, isDuplicateMsg: "Ya existe una empresa con ese nombre. Usa un nombre diferente.");
            }

            gvEmpresas.DataBind();
        }

        protected void SqlDataSourceEmpresa_Inserted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgEmpresas, isDuplicateMsg: "Ya existe una empresa con ese nombre. Usa un nombre diferente.");
                e.ExceptionHandled = true;
            }
        }

        protected void SqlDataSourceEmpresa_Updated(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgEmpresas, isDuplicateMsg: "Ya existe una empresa con ese nombre. Usa un nombre diferente.");
                e.ExceptionHandled = true;
            }
        }

        protected void SqlDataSourceEmpresa_Deleted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgEmpresas);
                e.ExceptionHandled = true;
            }
        }

        /* =========================================================
           PUESTOS
           ========================================================= */

        protected void btnAgregarPuesto_Click(object sender, EventArgs e)
        {
            ClearMessages();
            var nombre = (txtNuevoPuesto.Text ?? string.Empty).Trim();
            var desc   = (txtNuevoPuestoDesc.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                ShowError(lblMsgPuestos, "El nombre del puesto es obligatorio.");
                return;
            }

            try
            {
                SqlDataSourcePuesto.InsertParameters["Nombre"].DefaultValue = nombre;
                // Guardar NULL si la descripción llega vacía
                SqlDataSourcePuesto.InsertParameters["Descripcion"].DefaultValue =
                    string.IsNullOrWhiteSpace(desc) ? null : desc;

                SqlDataSourcePuesto.Insert();

                txtNuevoPuesto.Text = string.Empty;
                txtNuevoPuestoDesc.Text = string.Empty;
                ShowSuccess(lblMsgPuestos, "Puesto agregado correctamente.");
            }
            catch (Exception ex)
            {
                HandleSqlException(ex, lblMsgPuestos, isDuplicateMsg: "Ya existe un puesto con ese nombre. Usa un nombre diferente.");
            }

            gvPuestos.DataBind();
        }

        protected void SqlDataSourcePuesto_Inserted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgPuestos, isDuplicateMsg: "Ya existe un puesto con ese nombre. Usa un nombre diferente.");
                e.ExceptionHandled = true;
            }
        }

        protected void SqlDataSourcePuesto_Updated(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgPuestos, isDuplicateMsg: "Ya existe un puesto con ese nombre. Usa un nombre diferente.");
                e.ExceptionHandled = true;
            }
        }

        protected void SqlDataSourcePuesto_Deleted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgPuestos);
                e.ExceptionHandled = true;
            }
        }

        /* =========================================================
           SLA
           ========================================================= */

        protected void btnAgregarSla_Click(object sender, EventArgs e)
        {
            ClearMessages();

            var nombre = (txtNuevoSla.Text ?? string.Empty).Trim();
            var urgStr = (txtNuevoSlaUrgencia.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                ShowError(lblMsgSla, "El nombre del SLA es obligatorio.");
                return;
            }

            if (!int.TryParse(urgStr, out int urgencia))
            {
                ShowError(lblMsgSla, "La urgencia debe ser un número entre 1 y 10.");
                return;
            }
            if (urgencia < 1 || urgencia > 10)
            {
                ShowError(lblMsgSla, "La urgencia debe estar entre 1 y 10.");
                return;
            }

            try
            {
                SqlDataSourceSla.InsertParameters["Nombre"].DefaultValue = nombre;
                SqlDataSourceSla.InsertParameters["Urgencia"].DefaultValue = urgencia.ToString();
                SqlDataSourceSla.Insert();

                txtNuevoSla.Text = string.Empty;
                txtNuevoSlaUrgencia.Text = string.Empty;
                ShowSuccess(lblMsgSla, "SLA agregado correctamente.");
            }
            catch (Exception ex)
            {
                HandleSqlException(ex, lblMsgSla, isDuplicateMsg: "Ya existe un SLA con ese nombre. Usa un nombre diferente.");
            }

            gvSla.DataBind();
        }

        protected void SqlDataSourceSla_Inserted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgSla, isDuplicateMsg: "Ya existe un SLA con ese nombre. Usa un nombre diferente.");
                e.ExceptionHandled = true;
            }
        }

        protected void SqlDataSourceSla_Updated(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgSla, isDuplicateMsg: "Ya existe un SLA con ese nombre. Usa un nombre diferente.");
                e.ExceptionHandled = true;
            }
        }

        protected void SqlDataSourceSla_Deleted(object sender, System.Web.UI.WebControls.SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HandleSqlException(e.Exception, lblMsgSla);
                e.ExceptionHandled = true;
            }
        }

        /* =========================================================
           HELPERS
           ========================================================= */

        private void ClearMessages()
        {
            lblMsgEmpresas.Text = string.Empty;
            lblMsgPuestos.Text = string.Empty;
            lblMsgSla.Text = string.Empty;

            // Quita clases previas
            lblMsgEmpresas.CssClass = string.Empty;
            lblMsgPuestos.CssClass = string.Empty;
            lblMsgSla.CssClass = string.Empty;
        }

        private void ShowSuccess(System.Web.UI.WebControls.Label lbl, string message)
        {
            lbl.CssClass = "text-success";
            lbl.Text = message;
        }

        private void ShowError(System.Web.UI.WebControls.Label lbl, string message)
        {
            lbl.CssClass = "text-danger";
            lbl.Text = message;
        }

        private void HandleSqlException(Exception ex, System.Web.UI.WebControls.Label targetLabel, string isDuplicateMsg = null)
        {
            // Duplicado (índices únicos: UQ_hd_Empresa_Nombre, UQ_hd_Puesto_Nombre, UQ_hd_SLA_Nombre)
            if (ex is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                ShowError(targetLabel, isDuplicateMsg ?? "El registro ya existe con el mismo nombre.");
            }
            else
            {
                // Mensaje genérico (evitamos exponer detalles técnicos)
                ShowError(targetLabel, "No fue posible completar la operación. Verifica los datos e inténtalo de nuevo.");
            }
        }
    }
}