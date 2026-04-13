using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HelpDesk
{
    public partial class misTickets : UsuarioOnlyPage
    {
        private readonly string _strcon = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /* ========= Estética de la fila (sin event validation aquí) ========= */
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Style["cursor"] = "pointer";
                e.Row.ToolTip = "Clic para ver detalle";
            }
        }

        /* ========= Selección con 1 clic (postback seguro en Render) ========= */
        protected override void Render(HtmlTextWriter writer)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                var row = GridView1.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string postBack = Page.ClientScript.GetPostBackClientHyperlink(
                        GridView1, "Select$" + i, true
                    );
                    row.Attributes["onclick"] = postBack;
                    row.Style["cursor"] = "pointer";
                    row.ToolTip = "Clic para ver detalle";
                }
            }
            base.Render(writer);
        }

        /* ========= Al seleccionar una fila, mostramos detalle ========= */
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridView1.SelectedDataKey == null)
            {
                pnlDetalle.Visible = false;
                return;
            }

            int ticketId = Convert.ToInt32(GridView1.SelectedDataKey.Value);
            CargarDetalleTicket(ticketId);
        }

        private void CargarDetalleTicket(int ticketId)
        {
            using (var con = new SqlConnection(_strcon))
            using (var cmd = new SqlCommand(@"
                SELECT TOP 1
                      [TicketId]
                    , [UsuarioId]
                    , [Asunto]
                    , [Descripcion]
                    , [Estatus]
                    , [Prioridad]
                    , [AgenteId]
                    , [ParaUtc]
                    , [CreadoUtc]
                    , [ActualizadoUtc]
                    , [CerradoUtc]
                    , [Adjuntos]
                FROM [helpDeskDB].[hd].[Ticket]
                WHERE [TicketId] = @id;", con))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ticketId;
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                    {
                        pnlDetalle.Visible = false;
                        return;
                    }

                    // Identificadores
                    hidTicketId.Value = r["TicketId"].ToString();
                    lblTicketId.Text = "#" + r["TicketId"].ToString();

                    // Texto principal
                    txtAsunto.Text = NullToEmpty(r["Asunto"]);
                    txtEstatus.Text = NullToEmpty(r["Estatus"]);
                    txtPrioridad.Text = NullToEmpty(r["Prioridad"]);
                    txtAgenteId.Text = NullToEmpty(r["AgenteId"]);
                    txtUsuarioId.Text = NullToEmpty(r["UsuarioId"]);

                    // Fechas
                    txtParaUtc.Text = ToDateTimeStr(r["ParaUtc"]);
                    txtCreadoUtc.Text = ToDateTimeStr(r["CreadoUtc"]);
                    txtActualizadoUtc.Text = ToDateTimeStr(r["ActualizadoUtc"]);
                    txtCerradoUtc.Text = ToDateTimeStr(r["CerradoUtc"]);

                    // Descripción (multilínea)
                    txtDescripcion.Text = NullToEmpty(r["Descripcion"]);

                    // Adjuntos
                    var adj = NullToEmpty(r["Adjuntos"]);
                    lblAdjuntosRaw.Text = adj;
                    if (!string.IsNullOrWhiteSpace(adj))
                    {
                        lnkAdjuntos.NavigateUrl = SafeAttachmentUrl(adj);
                        lnkAdjuntos.Visible = true;
                    }
                    else
                    {
                        lnkAdjuntos.Visible = false;
                    }

                    // Badges: Estatus y Prioridad
                    PintarBadges(txtEstatus.Text, txtPrioridad.Text);

                    // Botón Cerrar: visible solo si NO está cerrado
                    btnCerrar.Visible = string.IsNullOrWhiteSpace(txtCerradoUtc.Text);

                    pnlDetalle.Visible = true;
                }
            }
        }

        private string SafeAttachmentUrl(string raw)
        {
            var val = (raw ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(val)) return "#";

            // 1) App-relative or site-root relative paths -> resolve
            if (val.StartsWith("~/") || val.StartsWith("/"))
                return ResolveUrl(val);

            // 2) Absolute HTTP/HTTPS
            Uri uri;
            if (Uri.TryCreate(val, UriKind.Absolute, out uri) &&
                (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp))
                return val;

            // 3) Plain file name (no slashes / backslashes)
            if (!val.Contains("/") && !val.Contains("\\"))
            {
                // Map to a known container in your site
                var safeName = Uri.EscapeDataString(val);
                return ResolveUrl("~/Uploads/Tickets/" + safeName);
            }

            // 4) Anything else -> block
            return "#";
        }

        private void PintarBadges(string estatus, string prioridad)
        {
            // ===== Estatus =====
            string estatusCss = "bg-primary";
            var estatusLower = (estatus ?? "").ToLowerInvariant();

            if (estatusLower.Contains("cerr"))                // "Cerrado"
                estatusCss = "bg-secondary";
            else if (estatusLower.Contains("abier"))         // "Abierto"
                estatusCss = "bg-primary";
            else if (estatusLower.Contains("proc") || estatusLower.Contains("aten")) // "En proceso"/"Atención"
                estatusCss = "bg-warning text-dark";
            else if (estatusLower.Contains("esc") || estatusLower.Contains("crit"))  // "Escalado"/"Crítico"
                estatusCss = "bg-danger";

            // ===== Prioridad (numérica 1/2/3 o texto) =====
            string prioCss = "bg-secondary";
            int p;
            if (int.TryParse((prioridad ?? "").Trim(), out p))
            {
                switch (p)
                {
                    case 1:
                        prioCss = "bg-danger";             // Alta
                        break;
                    case 2:
                        prioCss = "bg-warning text-dark";  // Media
                        break;
                    case 3:
                        prioCss = "bg-success";            // Baja
                        break;
                    default:
                        prioCss = "bg-secondary";
                        break;
                }
            }
            else
            {
                var pr = (prioridad ?? "").ToLowerInvariant();
                if (pr.Contains("alta")) prioCss = "bg-danger";
                else if (pr.Contains("medi")) prioCss = "bg-warning text-dark";
                else if (pr.Contains("baja")) prioCss = "bg-success";
            }

            lblEstatusBadge.Text = string.IsNullOrWhiteSpace(estatus) ? "—" : estatus;
            lblEstatusBadge.CssClass = "badge rounded-pill " + estatusCss;

            lblPrioridadBadge.Text = string.IsNullOrWhiteSpace(prioridad) ? "—" : prioridad;
            lblPrioridadBadge.CssClass = "badge rounded-pill " + prioCss;
        }


        /* ========= Botón Cerrar ticket ========= */
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hidTicketId.Value, out int id))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No hay TicketId válido.');", true);
                return;
            }

            try
            {
                using (var con = new SqlConnection(_strcon))
                using (var cmd = new SqlCommand(@"
                    UPDATE [helpDeskDB].[hd].[Ticket]
                    SET [CerradoUtc] = ISNULL([CerradoUtc], SYSUTCDATETIME()),
                        [ActualizadoUtc] = SYSUTCDATETIME()
                    WHERE [TicketId] = @id;", con))
                {
                    // Si quieres también forzar Estatus='Cerrado' (si es NVARCHAR), usa esta versión:
                    /*
                    using (var cmd = new SqlCommand(@"
                        UPDATE [helpDeskDB].[hd].[Ticket]
                        SET [CerradoUtc] = ISNULL([CerradoUtc], SYSUTCDATETIME()),
                            [ActualizadoUtc] = SYSUTCDATETIME(),
                            [Estatus] = CASE WHEN SQL_VARIANT_PROPERTY([Estatus],'BaseType') = 'nvarchar'
                                             THEN N'Cerrado' ELSE [Estatus] END
                        WHERE [TicketId] = @id;", con))
                    */
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    con.Open();
                    var rows = cmd.ExecuteNonQuery();
                }

                // Refrescar panel y grilla
                CargarDetalleTicket(id);
                GridView1.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "ok",
                    "alert('Ticket cerrado correctamente.');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err",
                    $"alert('Error al cerrar: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

        /* ========= Helpers ========= */
        protected string Truncate(object value, int max)
        {
            string text = (value ?? string.Empty).ToString();
            if (text.Length <= max) return text;
            return text.Substring(0, max) + "…";
        }

        protected string SafeImageUrl(object value)
        {
            string raw = (value ?? string.Empty).ToString().Trim();
            if (string.IsNullOrEmpty(raw))
                return ResolveUrl("~/img/placeholder.png");

            if (raw.StartsWith("~/") || raw.StartsWith("/"))
                return ResolveUrl(raw);

            if (Uri.TryCreate(raw, UriKind.Absolute, out Uri uri) &&
                uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
                return raw;

            return ResolveUrl("~/img/placeholder.png");
        }

        private static string NullToEmpty(object v) =>
            v == DBNull.Value ? "" : Convert.ToString(v);

        protected string ToDateTimeStr(object v)
        {
            if (v == DBNull.Value || v == null) return "";
            if (DateTime.TryParse(Convert.ToString(v), out var dt))
            {
                // Convertir de UTC a hora local
                DateTime localDate = ConvertUtcToLocal(dt);
                return localDate.ToString("dd/MM/yyyy HH:mm", CultureInfo.GetCultureInfo("es-MX"));
            }
            return Convert.ToString(v);
        }

        /// <summary>
        /// Convierte una fecha UTC a la hora local del servidor/usuario
        /// </summary>
        private DateTime ConvertUtcToLocal(DateTime utcDate)
        {
            // Si la fecha no tiene especificado que es UTC, asumimos que lo es
            DateTime dateToConvert = utcDate.Kind == DateTimeKind.Utc
                ? utcDate
                : DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);

            // Convertir a hora local
            return TimeZoneInfo.ConvertTimeFromUtc(dateToConvert, TimeZoneInfo.Local);
        }


        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count > 0)
            {
                GridView1.UseAccessibleHeader = true;                            // renders <th> for header cells
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;  // moves header to <thead>
                if (GridView1.FooterRow != null)
                    GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

    }
}
