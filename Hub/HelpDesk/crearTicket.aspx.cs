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
            if (string.IsNullOrWhiteSpace(sev)) return 4; // Normal
            sev = sev.Trim().ToLowerInvariant();
            switch (sev)
            {
                case "critico":
                case "crítico":
                    return 1;
                case "muy urgente":
                    return 2;
                case "urgente":
                    return 3;
                case "normal":
                    return 4;
                case "bajo":
                    return 5;
                default:
                    return 4;
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
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;

                    // 3) Parameters (strongly typed)

                    object creadoPor = Session["userid"] ??  (object)DBNull.Value;
                    if (creadoPor is string s && long.TryParse(s, out var creadoPorId))
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.BigInt).Value = creadoPorId;
                    else
                        cmd.Parameters.Add("@UsuarioId", SqlDbType.BigInt).Value = DBNull.Value;
                     
                    cmd.Parameters.Add("@Asunto", SqlDbType.NVarChar, 200).Value = asuntoValue;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, -1).Value = (object)descripcionValue ?? DBNull.Value; // -1 = NVARCHAR(MAX)
                    cmd.Parameters.Add("@Estatus", SqlDbType.Int).Value = 1;
                    // map severidad dropdown to prioridad numeric value (Critico=1 ... Bajo=5)
                    int prioridad = 4; // default Normal
                    try
                    {
                        var sev = DropDownListSeveridad?.SelectedValue ?? string.Empty;
                        prioridad = GetPrioridadFromSeveridad(sev);
                    }
                    catch { }
                    cmd.Parameters.Add("@Prioridad", SqlDbType.Int).Value = prioridad;
                    cmd.Parameters.Add("@AgenteId", SqlDbType.Int).Value = DBNull.Value;

                    // Nullable datetimes
                    cmd.Parameters.Add("@ParaUtc", SqlDbType.DateTime2).Value = DBNull.Value;
                    cmd.Parameters.Add("@Sla", SqlDbType.Int).Value = 1;

                    var nowUtc = DateTime.UtcNow;
                    cmd.Parameters.Add("@CreadoUtc", SqlDbType.DateTime2).Value = nowUtc;
                    cmd.Parameters.Add("@ActualizadoUtc", SqlDbType.DateTime2).Value = nowUtc;
                    cmd.Parameters.Add("@CerradoUtc", SqlDbType.DateTime2).Value = DBNull.Value;

                    cmd.Parameters.Add("@Adjuntos", SqlDbType.NVarChar, -1)
                       .Value = DBNull.Value;

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    newTicketId = Convert.ToInt32(result);


                    // 5) Save attachment under inventory/UserId/TicketId
                    if (FileUpload1.HasFile)
                    {


                        string ext = Path.GetExtension(FileUpload1.FileName).ToLowerInvariant();
                        string[] allowed = { ".jpg", ".png", ".pdf", ".docx", ".xlsx" };

                        if (!allowed.Contains(ext))
                        {
                            ShowClientMessage("Tipo de archivo no permitido.");
                            return;
                        }

                        if (FileUpload1.PostedFile.ContentLength > 30 * 1024 * 1024)
                        {
                            ShowClientMessage("Archivo demasiado grande (máx 30MB).");
                            return;
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

                        using (var con2 = new SqlConnection(_connString))
                        using (var cmd2 = new SqlCommand(updateSql, con2))
                        {
                            cmd2.Parameters.Add("@Adjuntos", SqlDbType.NVarChar, -1)
                                .Value = relativePath;

                            cmd2.Parameters.Add("@NowUtc", SqlDbType.DateTime2)
                                .Value = DateTime.UtcNow;

                            cmd2.Parameters.Add("@TicketId", SqlDbType.Int)
                                .Value = newTicketId;

                            con2.Open();
                            cmd2.ExecuteNonQuery();
                        }
                    }


                }

                // 4) Client feedback                 

                ShowClientMessageAndRedirect(
                    $"Ticket creado exitosamente. ID: {newTicketId}",
                    "InicioUsuario.aspx?registered=1"
                );

            }
            catch (SqlException sqlEx)
            {
                // TODO: log sqlEx via your logging framework (Serilog/NLog/ETW/Trace, etc.)
                ShowClientMessage("Ocurrió un error al crear el ticket. Intenta nuevamente. " + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                // TODO: log ex
                ShowClientMessage("Se produjo un error inesperado. Intenta nuevamente.");
            }
        }

       

    }
}