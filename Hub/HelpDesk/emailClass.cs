using HelpDesk.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace HelpDesk
{
    public class EmailService
    {
        private readonly string strcon =
            ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        /*
         * Send assignment / reassignment email directly (no queue)
         */
        public void QueueAssignmentEmail(
            string toEmail,
            int ticketId,
            string subject,
            string shortDescription,
            string priority,
            string ticketUrl,
            bool isReassignment)
        {
            if (string.IsNullOrWhiteSpace(toEmail)) return;

            try
            {
                string header = isReassignment
                    ? "Ticket reasignado"
                    : "Nuevo ticket asignado";

                string priorityColor = GetPriorityColor(priority);

                string body = $@"
                    <h3>{header}</h3>

                    <p>
                        <b>Ticket:</b> #{ticketId}<br/>
                        <b>Asunto:</b> {HttpUtility.HtmlEncode(subject)}<br/>
                        <b>Prioridad:</b>
                        <span style=""color:{priorityColor};font-weight:bold"">
                            {HttpUtility.HtmlEncode(priority)}
                        </span>
                    </p>

                    <p>
                        <b>Descripción:</b><br/>
                        {HttpUtility.HtmlEncode(shortDescription)}
                    </p>

                    <p>
                        <a href=""{ticketUrl}""
                           style=""background:#0d6efd;color:#fff;
                                  padding:8px 14px;
                                  text-decoration:none;
                                  border-radius:4px;"">
                            Ver ticket
                        </a>
                    </p>

                    <hr/>
                    <small>Mensaje automático del sistema HelpDesk</small>
                ";

                // Send directly using stored procedure
                SendEmailDirect(toEmail, $"{header} #{ticketId}: {subject}", body);
            }
            catch (Exception ex)
            {
                 Logger.RegistrarError($"Error enviando el email de reasignación: {ex.Message}");

                // Don't throw - email failure shouldn't break ticket assignment
            }
        }

        /*
         * Send email directly using stored procedure (requires SQL Server Database Mail configured)
         */
        private void SendEmailDirect(string toEmail, string subject, string body)
        {
            try
            {
                using (var cn = new SqlConnection(strcon))
                using (var cmd = cn.CreateCommand())
                {
                    // Call your stored procedure directly
                    cmd.CommandText = "dbo.SendQueuedEmail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30;

                    cmd.Parameters.AddWithValue("@ToEmail", toEmail);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Body", body);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    Logger.RegistrarInfo($"Email enviado con exito a: {toEmail}");
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError($"Error en SendEmailDirect: {ex.Message}");
                throw;
            }
        }

        /*
         * Priority → color mapping
         */
        private string GetPriorityColor(string priority)
        {
            if (string.IsNullOrWhiteSpace(priority)) return "#6c757d"; // gray

            switch (priority.ToLower())
            {
                case "alta":
                case "critica":
                    return "#dc3545"; // red

                case "media":
                    return "#fd7e14"; // orange

                case "baja":
                    return "#198754"; // green

                default:
                    return "#0d6efd"; // blue
            }
        }
    }
}
