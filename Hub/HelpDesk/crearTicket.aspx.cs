using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.UI;

namespace HelpDesk
{
    public partial class WebForm1 : UsuarioOnlyPage
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }


        private int GetPrioridadFromSeveridad(string sev)
        {
            if (string.IsNullOrWhiteSpace(sev)) return 1;
            sev = sev.Trim().ToLowerInvariant();
            switch (sev)
            {
                case "critico":
                case "crítico":
                    return 8;
                case "muy urgente":
                    return 4;
                case "urgente":
                    return 2;
                case "normal":
                    return 1;
                case "bajo":
                    return 0;
                default:
                    return 1;
            }
        }

        private int? GetAvailableAgent(int slaId, SqlConnection con, SqlTransaction tx)
        {
            const string sql = @"
            SELECT TOP 1 A.agenteId
            FROM hd.Agente A WITH (UPDLOCK, READPAST)
            JOIN hd.SLA S ON S.SLAId = @SlaId
            WHERE
                A.Estatus = 1
            ORDER BY
                ISNULL(A.tAbiertos, 0) ASC,
            A.agenteId ASC;";

            using (var cmd = new SqlCommand(sql, con, tx))
            {
                cmd.Parameters.Add("@SlaId", SqlDbType.Int).Value = slaId;
                object result = cmd.ExecuteScalar();
                return result == null ? (int?)null : Convert.ToInt32(result);
            }
        }

        
        private int GetSlaUrgencia(int slaId, SqlConnection con, SqlTransaction tx)
        {
            using (var cmd = new SqlCommand(
                "SELECT TiempoDeRespuesta FROM hd.SLA WHERE SLAId = @SlaId",
                con, tx))
            {
                cmd.Parameters.Add("@SlaId", SqlDbType.Int).Value = slaId;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }



        protected void CrearTicket_Click(object sender, EventArgs e)
        {
            // 1) Validate inputs
            string asuntoValue = (asunto?.Text ?? string.Empty).Trim();
            string descripcionValue = (descripcion?.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(asuntoValue))
            {
                ShowClientMessage("Por favor ingresa el asunto.");
                return;
            }

            // Bound the length to match DB.
            if (asuntoValue.Length > 300)
            {
                asuntoValue = asuntoValue.Substring(0, 300);
            }

            // descripcionValue is NVARCHAR(MAX); still considers a maximum to protect the system.
            const int maxDescripcion = 8000;
            if (descripcionValue.Length > maxDescripcion)
            {
                descripcionValue = descripcionValue.Substring(0, maxDescripcion);
            }

            // 2) Prepare SQL (return new Id)
            const string sql = @"
                                INSERT INTO hd.Ticket
                                (
                                    UsuarioId,
                                    Asunto,
                                    Descripcion,
                                    Estatus,
                                    Prioridad,
                                    AgenteId,
                                    ParaUtc,
                                    Sla,
                                    CreadoUtc,
                                    ActualizadoUtc,
                                    CerradoUtc,
                                    Adjuntos
                                )
                                OUTPUT INSERTED.TicketId
                                VALUES
                                (
                                    @UsuarioId,
                                    @Asunto,
                                    @Descripcion,
                                    @Estatus,
                                    @Prioridad,
                                    @AgenteId,
                                    @ParaUtc,
                                    @Sla,
                                    @CreadoUtc,
                                    @ActualizadoUtc,
                                    @CerradoUtc,
                                    @Adjuntos
                                );";

            try
            {
                int newTicketId;

                using (var con = new SqlConnection(_connString))
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        try
                        {

                            using (var cmd = new SqlCommand(sql, con, tx))
                            {
                                cmd.CommandType = CommandType.Text;

                                // 3) Parameters (strongly typed)


                                if (!(Session["userid"] is string s))
                                    throw new Exception("Usuario no válido.");

                                if (!long.TryParse(s, out long creadoPorId))
                                    throw new Exception("Usuario no válido.");

                                cmd.Parameters.Add("@UsuarioId", SqlDbType.BigInt).Value = creadoPorId;
                                
                                

                                cmd.Parameters.Add("@Asunto", SqlDbType.NVarChar, 200).Value = asuntoValue;
                                cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value = (object)descripcionValue ?? DBNull.Value; // -1 = NVARCHAR(MAX)
                                cmd.Parameters.Add("@Estatus", SqlDbType.Int).Value = 1;
                                // map severidad dropdown to prioridad numeric value
                                int prioridad = 1; // default Normal
                                try
                                {
                                    var sev = DropDownListSeveridad?.SelectedValue ?? string.Empty;
                                    prioridad = GetPrioridadFromSeveridad(sev);
                                }
                                catch { }
                                cmd.Parameters.Add("@Prioridad", SqlDbType.Int).Value = prioridad;                                 

                                int slaId;
                                using (var slaCmd = new SqlCommand(@"
                                                                    SELECT ISNULL(SLA, 1)
                                                                    FROM hd.Usuario
                                                                    WHERE UsuarioId = @UsuarioId", con, tx))
                                {
                                    slaCmd.Parameters.Add("@UsuarioId", SqlDbType.BigInt).Value = creadoPorId;
                                    slaId = Convert.ToInt32(slaCmd.ExecuteScalar());
                                }

                                int slaUrgencia = GetSlaUrgencia(slaId, con, tx);

                                // Protect against invalid data
                                if (prioridad <= 0) prioridad = 1;

                                // Formula: now + (SLA.Urgencia / Prioridad) minutes
                                DateTime paraUtc =
                                    DateTime.UtcNow.AddMinutes(
                                        (double)slaUrgencia / prioridad);
                                
                                int ? agenteId = GetAvailableAgent(slaId, con, tx);

                                cmd.Parameters.Add("@AgenteId", SqlDbType.Int).Value = (object)agenteId ?? DBNull.Value;

                                // Nullable datetimes
                                cmd.Parameters.Add("@ParaUtc", SqlDbType.DateTime2).Value = paraUtc;
                                cmd.Parameters.Add("@Sla", SqlDbType.Int).Value = slaId;

                                var nowUtc = DateTime.UtcNow;
                                cmd.Parameters.Add("@CreadoUtc", SqlDbType.DateTime2).Value = nowUtc;
                                cmd.Parameters.Add("@ActualizadoUtc", SqlDbType.DateTime2).Value = nowUtc;
                                cmd.Parameters.Add("@CerradoUtc", SqlDbType.DateTime2).Value = DBNull.Value;

                                cmd.Parameters.Add("@Adjuntos", SqlDbType.NVarChar, -1)
                                   .Value = DBNull.Value;


                                object result = cmd.ExecuteScalar();
                                newTicketId = Convert.ToInt32(result);


                                if (agenteId.HasValue)
                                {
                                    using (var inc = new SqlCommand(@"
                                    UPDATE hd.Agente
                                    SET tAbiertos = ISNULL(tAbiertos,0) + 1
                                    WHERE agenteId = @AgenteId", con, tx))
                                    {
                                        inc.Parameters.Add("@AgenteId", SqlDbType.Int).Value = agenteId.Value;
                                        inc.ExecuteNonQuery();
                                    }
                                }



                                // 5) Save attachment under inventory/UserId/TicketId
                                if (FileUpload1.HasFile)
                                {

                                    string ext = Path.GetExtension(FileUpload1.FileName).ToLowerInvariant();
                                    string[] allowed = { ".jpg", ".png", ".pdf", ".docx", ".xlsx" };

                                    if (!allowed.Contains(ext))
                                    {
                                        throw new Exception("Tipo de archivo no permitido.");
                                    }

                                    if (FileUpload1.PostedFile.ContentLength > 30 * 1024 * 1024)
                                    { 
                                        throw new Exception("Archivo demasiado grande (máx 30MB).");
                                    }

                                    // Get userId safely
                                    long userId = 0;
                                    if (Session["userid"] is string ss && long.TryParse(ss, out var uid))
                                        userId = uid;

                                    string fileName = Path.GetFileName(FileUpload1.FileName);


                                    string uniqueName =
                                        Path.GetFileNameWithoutExtension(fileName)
                                        + "_" + Guid.NewGuid()
                                        + Path.GetExtension(fileName);


                                    // ~/inventory/{UserId}/{TicketId}/
                                    string relativeDir = $"~/inventory/{userId}/{newTicketId}/";
                                    string physicalDir = Server.MapPath(relativeDir);

                                    // Create directory if not exists
                                    if (!Directory.Exists(physicalDir))
                                    {
                                        Directory.CreateDirectory(physicalDir);
                                    }

                                    string relativePath = relativeDir + uniqueName;
                                    string physicalPath = Path.Combine(physicalDir, uniqueName);

                                    // Save file
                                    FileUpload1.SaveAs(physicalPath);

                                    // 6) Update ticket with attachment path
                                    const string updateSql = @"
                                    UPDATE hd.Ticket
                                    SET Adjuntos = @Adjuntos,
                                        ActualizadoUtc = @NowUtc
                                    WHERE TicketId = @TicketId";

                                    using (var cmd2 = new SqlCommand(updateSql, con, tx))
                                    {
                                        cmd2.Parameters.Add("@Adjuntos", SqlDbType.NVarChar, -1)
                                            .Value = relativePath;

                                        cmd2.Parameters.Add("@NowUtc", SqlDbType.DateTime2)
                                            .Value = DateTime.UtcNow;

                                        cmd2.Parameters.Add("@TicketId", SqlDbType.Int)
                                            .Value = newTicketId;

                                        cmd2.ExecuteNonQuery();
                                    }
                                }


                            }
                                          

                            tx.Commit();
                            ShowClientMessageAndRedirect(
                                $"Ticket creado exitosamente. ID: {newTicketId}",
                                "InicioUsuario.aspx?registered=1"
                            );
                        }
                        catch (Exception)
                        { 
                            tx.Rollback();
                            throw;
                        }

                    }
                }
            }

            catch (SqlException sqlEx)
            {
                // TODO: log sqlEx via your logging framework (Serilog/NLog/ETW/Trace, etc.)
                ShowClientMessage("Ocurrió un error al crear el ticket. Intenta nuevamente. " + sqlEx.ToString());
            }
        }
    }

}

              
                