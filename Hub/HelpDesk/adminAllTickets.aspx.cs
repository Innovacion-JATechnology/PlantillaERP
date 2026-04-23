using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HelpDesk.Utilities;

namespace HelpDesk
{
    public partial class adminAllTickets : AgentPage
    {
        private readonly string strcon =
            ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSearch.Text = string.Empty;
                txtFechaFrom.Text = string.Empty;
                txtFechaTo.Text = string.Empty;
                lbEstatus.ClearSelection();
                LoadTickets(null, null, null, null);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTickets(txtSearch.Text, null, null, null);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            txtFechaFrom.Text = string.Empty;
            txtFechaTo.Text = string.Empty;
            lbEstatus.ClearSelection();
            LoadTickets(null, null, null, null);
        }

        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text;
            string fechaFrom = txtFechaFrom.Text;
            string fechaTo = txtFechaTo.Text;

            // Get selected status values
            List<string> selectedStatuses = lbEstatus.Items.Cast<ListItem>()
                .Where(x => x.Selected)
                .Select(x => x.Value)
                .ToList();

            string estatus = selectedStatuses.Count > 0 ? string.Join(",", selectedStatuses) : null;

            LoadTickets(searchText, fechaFrom, fechaTo, estatus);
        }

        private void LoadTickets(string q, string dateFrom, string dateTo, string status)
        {
            string baseSql = @"
SELECT t.TicketId,
       u.nombre + ' ' + ISNULL(u.ApPaterno,'') AS UsuarioNombre,
       t.CreadoUtc,
       t.AgenteId,
       a.nombre AS AgenteNombre,
       ISNULL(a.estatus, 1) AS AgenteEstatus,
       t.Estatus,
       t.Asunto,
       t.Descripcion,
       t.Prioridad
FROM [hd].[Ticket] t
LEFT JOIN [hd].[Usuario] u ON t.UsuarioId = u.UsuarioId
LEFT JOIN [hd].[Agente] a ON t.AgenteId = a.agenteId
";

            SqlDataSource1.SelectParameters.Clear();
            List<string> whereConditions = new List<string>();

            // Build WHERE clause based on filters
            if (!string.IsNullOrWhiteSpace(q))
            {
                whereConditions.Add("(t.Asunto LIKE '%' + @q + '%' OR t.Descripcion LIKE '%' + @q + '%')");
                SqlDataSource1.SelectParameters.Add("q", q);
            }

            if (!string.IsNullOrWhiteSpace(dateFrom))
            {
                if (DateTime.TryParse(dateFrom, out DateTime fromDate))
                {
                    // Convert local date to UTC start of day
                    DateTime utcFromDate = TimeZoneInfo.ConvertTimeToUtc(fromDate, TimeZoneInfo.Local);
                    whereConditions.Add("t.CreadoUtc >= @dateFrom");
                    SqlDataSource1.SelectParameters.Add("dateFrom", utcFromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (!string.IsNullOrWhiteSpace(dateTo))
            {
                if (DateTime.TryParse(dateTo, out DateTime toDate))
                {
                    // Convert local date to UTC end of day
                    DateTime utcToDate = TimeZoneInfo.ConvertTimeToUtc(toDate.AddDays(1).AddSeconds(-1), TimeZoneInfo.Local);
                    whereConditions.Add("t.CreadoUtc <= @dateTo");
                    SqlDataSource1.SelectParameters.Add("dateTo", utcToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                // Status is now a comma-separated string of status values (e.g., "1,2,3")
                string[] statusValues = status.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (statusValues.Length > 0)
                {
                    // Build IN clause: t.Estatus IN (1, 2, 3)
                    List<string> statusPlaceholders = new List<string>();
                    for (int i = 0; i < statusValues.Length; i++)
                    {
                        string paramName = "status" + i;
                        statusPlaceholders.Add("@" + paramName);
                        SqlDataSource1.SelectParameters.Add(paramName, statusValues[i]);
                    }

                    whereConditions.Add("t.Estatus IN (" + string.Join(", ", statusPlaceholders) + ")");
                }
            }

            // Combine WHERE conditions
            string whereClause = string.Empty;
            if (whereConditions.Count > 0)
            {
                whereClause = " WHERE " + string.Join(" AND ", whereConditions);
            }

            SqlDataSource1.SelectCommand = baseSql + whereClause + " ORDER BY t.CreadoUtc DESC";

            GridView1.PageIndex = 0;
            GridView1.DataBind();
        }

       protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
       {
           if (e.Row.RowType != DataControlRowType.DataRow) return;

           var drv = (System.Data.DataRowView)e.Row.DataItem;

           e.Row.Attributes["data-ticketid"] = drv["TicketId"].ToString();
           e.Row.Attributes["data-asunto"] = drv["Asunto"].ToString();
           e.Row.Attributes["data-descripcion"] = drv["Descripcion"].ToString();
           e.Row.Attributes["data-prioridad"] = drv["Prioridad"].ToString();
           e.Row.Attributes["data-agenteid"] = drv["AgenteId"]?.ToString() ?? "";
           e.Row.Attributes["data-estatus"] = drv["Estatus"]?.ToString() ?? "";
           e.Row.Attributes["data-ticketstatus"] = drv["Estatus"]?.ToString() ?? "1";

           // Convert UTC date to local time for display
            if (drv["CreadoUtc"] != DBNull.Value && DateTime.TryParse(drv["CreadoUtc"].ToString(), out DateTime creadoUtcDate))
            {
                DateTime localDate = ConvertUtcToLocal(creadoUtcDate);
                e.Row.Attributes["data-creado"] = localDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                e.Row.Attributes["data-creado"] = "";
            }

            e.Row.Attributes["style"] = "cursor:pointer;";
            e.Row.Attributes["onclick"] = "rowClick(this)";

            // Format Estatus column to show user-friendly text with colors
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                string headerText = GridView1.HeaderRow.Cells[i].Text;

                // Format status column
                if (headerText == "Estatus")
                {
                    string statusValue = drv["Estatus"]?.ToString() ?? "1";
                    int statusNum = int.Parse(statusValue);
                    string displayText = TicketStatusTransitions.GetStatusDisplayName(statusNum);

                    // Apply color styling based on status
                    string bgColor = GetStatusColor(statusNum);
                    string textColor = GetStatusTextColor(statusNum);

                    e.Row.Cells[i].Text = displayText;
                    e.Row.Cells[i].Style["background-color"] = bgColor;
                    e.Row.Cells[i].Style["color"] = textColor;
                    e.Row.Cells[i].Style["font-weight"] = "bold";
                    e.Row.Cells[i].Style["text-align"] = "center";
                    e.Row.Cells[i].Style["border-radius"] = "4px";
                    e.Row.Cells[i].Style["padding"] = "5px";
                }
            }
       }

       private string GetStatusColor(int status)
       {
           switch (status)
           {
               case 1: return "#E3F2FD"; // Nuevo - Azul claro
               case 2: return "#BBDEFB"; // Abierto - Azul
               case 3: return "#FFE0B2"; // En Progreso - Naranja claro
               case 4: return "#FFF9C4"; // En Espera - Amarillo claro
               case 5: return "#FFCCBC"; // Escalado - Rojo claro
               case 6: return "#C8E6C9"; // Resuelto - Verde claro
               case 7: return "#81C784"; // Cerrado - Verde oscuro
               case 8: return "#CE93D8"; // Reabierto - Púrpura
               case 9: return "#E0E0E0"; // Cancelado - Gris
               default: return "#FFFFFF"; // Blanco
           }
       }

       private string GetStatusTextColor(int status)
       {
           switch (status)
           {
               case 1: return "#0D47A1"; // Azul oscuro
               case 2: return "#1565C0"; // Azul oscuro
               case 3: return "#E65100"; // Naranja oscuro
               case 4: return "#F57F17"; // Amarillo oscuro
               case 5: return "#D84315"; // Rojo oscuro
               case 6: return "#2E7D32"; // Verde oscuro
               case 7: return "#FFFFFF"; // Blanco
               case 8: return "#6A1B9A"; // Púrpura oscuro
               case 9: return "#424242"; // Gris oscuro
               default: return "#000000"; // Negro
           }
       }
       

        protected void modalBtnAssign_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(modalHiddenTicketId.Value, out int ticketId)) return;
            if (!int.TryParse(modalDdlAgents.SelectedValue, out int agenteId)) return;

            try
            {
                // ✅ Assign ticket and capture previous agent
                int? previousAgent = AssignTicketToAgent(ticketId, agenteId);
                bool reassigned = previousAgent.HasValue && previousAgent.Value != agenteId;

                Logger.RegistrarInfo($"Ticket {ticketId} asignado a Agente {agenteId}. Reasignado: {reassigned}");

                // ✅ Email data
                string agentEmail = GetAgentEmail(agenteId);
                var info = GetTicketInfo(ticketId);
                string ticketUrl = GetTicketUrl(ticketId);

                // ✅ Queue email (SQL Server Mail)
                try
                {
                    var emailSvc = new EmailService();
                    emailSvc.QueueAssignmentEmail(
                        agentEmail,
                        ticketId,
                        info.Subject,
                        info.ShortDescription,
                        info.Priority,
                        ticketUrl,
                        reassigned
                    );
                    Logger.RegistrarInfo($"Email enlistado exitosamente ticket {ticketId} para {agentEmail}");
                }
                catch (Exception emailEx)
                {
                    // Log email error but don't fail the assignment
                    Logger.RegistrarError($"Error enviando email del ticket {ticketId} a {agentEmail}", emailEx);
                    ScriptManager.RegisterStartupScript(this, GetType(), "emailError",
                        $"alert('Ticket asignado pero error al enviar correo: {emailEx.Message}');", true);
                }

                // ✅ Redirect to refresh page cleanly without postback
                Response.Redirect(Request.Url.ToString());
            }
            catch (Exception ex)
            {
                // Handle assignment errors
                Logger.RegistrarError($"Error asignando ticket {modalHiddenTicketId.Value} a agente {modalDdlAgents.SelectedValue}", ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "assignError",
                    $"alert('Error al asignar ticket: {ex.Message}');", true);
            }
        }

        protected void modalBtnUpdateTicketStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(modalHiddenTicketId.Value, out int ticketId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "parseError",
                        "alert('Error: TicketId inválido');", true);
                    return;
                }

                if (!int.TryParse(modalDdlTicketStatus.SelectedValue, out int newStatusValue))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "parseError",
                        "alert('Error: Estado inválido');", true);
                    return;
                }

                // Get current ticket status
                var currentStatus = GetCurrentTicketStatus(ticketId);
                var newStatus = (TicketStatus)newStatusValue;

                // Get user role
                string userRole = Session["role"]?.ToString() ?? "usuario";
                string userName = Session["username"]?.ToString() ?? "Unknown";

                Logger.RegistrarInfo($"[UpdateStatus] TicketId: {ticketId}, CurrentStatus: {currentStatus}, NewStatus: {newStatus}, UserRole: {userRole}");

                // Validate transition based on role
                bool canTransition = TicketStatusTransitions.CanTransitionTo(currentStatus, newStatus, userRole);
                Logger.RegistrarInfo($"[UpdateStatus] CanTransition: {canTransition}");

                if (!canTransition)
                {
                    string currentStatusName = TicketStatusTransitions.GetStatusDisplayName(currentStatus);
                    string newStatusName = TicketStatusTransitions.GetStatusDisplayName(newStatus);
                    Logger.RegistrarInfo($"[UpdateStatus] Transición NO permitida de {currentStatusName} a {newStatusName} para rol {userRole}");
                    ScriptManager.RegisterStartupScript(this, GetType(), "invalidTransition",
                        $"alert('No se puede cambiar de {currentStatusName} a {newStatusName}');", true);
                    return;
                }

                // Update the status
                UpdateTicketStatus(ticketId, (int)newStatus);

                // Log the status change
                string statusChangeLog = $"Ticket {ticketId}: {TicketStatusTransitions.GetStatusDisplayName(currentStatus)} → {TicketStatusTransitions.GetStatusDisplayName(newStatus)} (Usuario: {userName}, Rol: {userRole})";
                Logger.RegistrarInfo(statusChangeLog);

                // Show success message
                ScriptManager.RegisterStartupScript(this, GetType(), "successMsg",
                    "alert('Estado actualizado correctamente');", true);

                // Refresh the grid
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Logger.RegistrarError($"Error updating status for ticket {modalHiddenTicketId.Value}", ex);
                ScriptManager.RegisterStartupScript(this, GetType(), "statusError",
                    $"alert('Error al actualizar estatus: {ex.Message}');", true);
            }
        }

        /* =========================
           DATA HELPERS
           ========================= */

        private string GetAgentEmail(int agenteId)
        {
            using (var cn = new SqlConnection(strcon))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT email FROM [hd].[Agente] WHERE agenteId = @id";
                cmd.Parameters.AddWithValue("@id", agenteId);
                cn.Open();
                return cmd.ExecuteScalar()?.ToString();
            }
        }

        private (string Subject, string ShortDescription, string Priority)
            GetTicketInfo(int ticketId)
        {
            using (var cn = new SqlConnection(strcon))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT Asunto, Descripcion, Prioridad
FROM [hd].[Ticket]
WHERE TicketId = @id";
                cmd.Parameters.AddWithValue("@id", ticketId);

                cn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        string subject = r["Asunto"]?.ToString() ?? "";
                        string desc = r["Descripcion"]?.ToString() ?? "";
                        string priority = r["Prioridad"]?.ToString() ?? "Normal";
                        return (subject, FirstWords(desc, 40), priority);
                    }
                }
            }
            return ("", "", "Normal");
        }

        private string FirstWords(string text, int maxWords)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";

            var words = text.Split(
                new[] { ' ', '\r', '\n', '\t' },
                StringSplitOptions.RemoveEmptyEntries);

            if (words.Length <= maxWords)
                return string.Join(" ", words);

            return string.Join(" ", words.Take(maxWords)) + "…";
        }

        private string GetTicketUrl(int ticketId)
        {
            var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            return $"{baseUrl}{ResolveUrl($"~/AdminAllTickets.aspx?ticketId={ticketId}")}";
        }

        private void UpdateAgentStatus(int agenteId, int estatus)
        {
            using (var cn = new SqlConnection(strcon))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE [hd].[Agente]
SET estatus = @estatus
WHERE agenteId = @id";
                cmd.Parameters.AddWithValue("@estatus", estatus);
                cmd.Parameters.AddWithValue("@id", agenteId);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateTicketStatus(int ticketId, int estatus)
        {
            try
            {
                using (var cn = new SqlConnection(strcon))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
UPDATE [hd].[Ticket]
SET Estatus = @estatus
WHERE TicketId = @id";
                    cmd.Parameters.AddWithValue("@estatus", estatus);
                    cmd.Parameters.AddWithValue("@id", ticketId);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception($"No se actualizó ningún ticket. TicketId: {ticketId}");
                    }

                    Logger.RegistrarInfo($"Ticket {ticketId} actualizado exitosamente a estado {estatus}");
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError($"Error en UpdateTicketStatus para ticket {ticketId}: {ex.Message}", ex);
                throw; // Re-lanzar para que se maneje en el método llamador
            }
        }

        /// <summary>
        /// WebMethod to get valid next states for a ticket based on current status and user role
        /// Used by JavaScript to populate dropdown dynamically
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<TicketStatusOption> GetValidTicketStatuses(int currentStatusValue)
        {
            var result = new List<TicketStatusOption>();

            try
            {
                // Get current HTTP context to access Session
                HttpContext context = HttpContext.Current;
                string userRole = context?.Session?["role"]?.ToString() ?? "usuario";

                // Log para debugging
                Logger.RegistrarInfo($"GetValidTicketStatuses - CurrentStatus: {currentStatusValue}, UserRole: {userRole}");

                var currentStatus = (TicketStatus)currentStatusValue;
                var validStates = TicketStatusTransitions.GetValidNextStates(currentStatus, userRole);

                foreach (var state in validStates)
                {
                    result.Add(new TicketStatusOption
                    {
                        Value = (int)state,
                        Text = TicketStatusTransitions.GetStatusDisplayName(state)
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError($"Error en GetValidTicketStatuses: {ex.Message}", ex);
            }

            return result;
        }

        public class TicketStatusOption
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        private TicketStatus GetCurrentTicketStatus(int ticketId)
        {
            using (var cn = new SqlConnection(strcon))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT Estatus FROM [hd].[Ticket] WHERE TicketId = @id";
                cmd.Parameters.AddWithValue("@id", ticketId);
                cn.Open();
                var result = cmd.ExecuteScalar();
                return (TicketStatus)Convert.ToInt32(result ?? 1);
            }
        }

        /* =========================
           ASSIGNMENT (returns previous agent)
           ========================= */

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Intentionally empty
            // Selection handled via RowDataBound + JS modal
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Not used (assignment via modal)
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }



        private int? AssignTicketToAgent(int ticketId, int agenteId)
        {
            int? previousAgent = null;

            using (var cn = new SqlConnection(strcon))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                using (var cmd = cn.CreateCommand())
                {
                    cmd.Transaction = tx;

                    // Read current agent
                    cmd.CommandText =
                        "SELECT AgenteId FROM [hd].[Ticket] WHERE TicketId = @id";
                    cmd.Parameters.AddWithValue("@id", ticketId);

                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                        previousAgent = Convert.ToInt32(obj);

                    // Decrement old agent counter
                    if (previousAgent.HasValue && previousAgent.Value != agenteId)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = @"
UPDATE [hd].[Agente]
SET tAbiertos = CASE WHEN tAbiertos > 0 THEN tAbiertos - 1 ELSE 0 END
WHERE agenteId = @id";
                        cmd.Parameters.AddWithValue("@id", previousAgent.Value);
                        cmd.ExecuteNonQuery();
                    }

                    // Assign new agent
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"
UPDATE [hd].[Ticket]
SET AgenteId = @agenteId,
    ActualizadoUtc = SYSUTCDATETIME()
WHERE TicketId = @ticketId";
                    cmd.Parameters.AddWithValue("@agenteId", agenteId);
                    cmd.Parameters.AddWithValue("@ticketId", ticketId);
                    cmd.ExecuteNonQuery();

                    // Increment new agent counter
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"
UPDATE [hd].[Agente]
SET tAbiertos = ISNULL(tAbiertos,0) + 1
WHERE agenteId = @id";
                    cmd.Parameters.AddWithValue("@id", agenteId);
                    cmd.ExecuteNonQuery();

                    tx.Commit();
                }
            }

            return previousAgent;
        }

        /// <summary>
        /// Convierte una fecha UTC a la hora local del servidor/usuario
        /// </summary>
        protected DateTime ConvertUtcToLocal(DateTime utcDate)
        {
            // Si la fecha no tiene especificado que es UTC, asumimos que lo es
            DateTime dateToConvert = utcDate.Kind == DateTimeKind.Utc
                ? utcDate
                : DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);

            // Convertir a hora local
            return TimeZoneInfo.ConvertTimeFromUtc(dateToConvert, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Convierte una fecha UTC a formato local de texto
        /// </summary>
        protected string FormatDateLocal(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return "";

            if (DateTime.TryParse(dateValue.ToString(), out DateTime date))
            {
                DateTime localDate = ConvertUtcToLocal(date);
                return localDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return "";
        }

        /// <summary>
        /// Convierte un número de prioridad a su etiqueta legible
        /// </summary>
        public string GetPriorityLabel(object priorityValue)
        {
            if (priorityValue == null || priorityValue == DBNull.Value)
                return "Normal";

            if (int.TryParse(priorityValue.ToString(), out int priority))
            {
                switch (priority)
                {
                    case 8: return "Crítico";
                    case 4: return "Muy urgente";
                    case 2: return "Urgente";
                    case 1: return "Normal";
                    case 0: return "Bajo";
                    default: return "Normal";
                }
            }
            return "Normal";
        }
    }
}
