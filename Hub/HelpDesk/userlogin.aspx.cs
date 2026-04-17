using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;

using HelpDesk.Security;
using HelpDesk.Utilities;

namespace HelpDesk
{
    public partial class userlogin : PaginasHD
    {
        private readonly string strcon = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Continuar_LogIn_Click(object sender, EventArgs e)
        {
            try
            {
                var userEmail = usuario.Text == null ? "" : usuario.Text.Trim().ToLowerInvariant();
                var pwd = contrasena.Text == null ? "" : contrasena.Text;

                if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(pwd))
                {
                    ShowClientMessage("Por favor ingresa correo y contraseña.");
                    return;
                }

                using (var con = new SqlConnection(strcon))
                using (var cmd = new SqlCommand(@"
                    SELECT TOP 1
                        UsuarioId,
                        Nombre,
                        Correo,
                        Activo,
                        Estatus,
                        PasswordHash,
                        PasswordSalt
                    FROM hd.Usuario
                    WHERE Correo = @Correo;
                ", con))
                {
                    cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 256).Value = userEmail;

                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows)
                        {
                            ShowClientMessage("Credenciales inválidas.");
                            return;
                        }

                        if (rdr.Read())
                        {
                            var activo = rdr["Activo"] != DBNull.Value && (bool)rdr["Activo"];
                            var estatus = rdr["Estatus"] != DBNull.Value && (bool)rdr["Estatus"];

                            // Obtener salt y hash almacenados
                            var dbSalt = rdr["PasswordSalt"] as byte[];
                            var dbHash = rdr["PasswordHash"] as byte[];

                            // Validar tamaños esperados
                            if (dbSalt == null || dbHash == null ||
                                dbSalt.Length != PasswordCrypto.SaltSize ||
                                dbHash.Length != PasswordCrypto.HashSize)
                            {
                                ShowClientMessage("Error de credenciales (formato inválido).");
                                return;
                            }

                            // Verificar contraseña
                            var ok = PasswordCrypto.VerifyPassword(pwd, dbSalt, dbHash);
                            if (!ok)
                            {
                                ShowClientMessage("Credenciales inválidas.");
                                return;
                            }

                            if (!activo || !estatus)
                            {
                                ShowClientMessage("Tu cuenta está inactiva. Contacta al administrador.");
                                return;
                            }

                            // Autenticación OK → setear sesión
                            Session["userid"] = rdr["UsuarioId"].ToString();
                            Session["username"] = rdr["Correo"].ToString();
                            Session["fullname"] = rdr["Nombre"].ToString();
                            Session["role"] = "usuario";
                            Session["status"] = estatus ? "Activo" : "Inactivo";

                            // Redirigir
                            Response.Redirect("InicioUsuario.aspx", false);
                            Logger.RegistrarInfo($"Usuario '{userEmail}' inició sesión exitosamente.");
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Evita exponer detalles técnicos al usuario final
                ShowClientMessage("Ocurrió un error al iniciar sesión.");
                Logger.RegistrarError("Error en proceso de login para usuario: " + (usuario.Text ?? "N/A"));
            }
        }

        private void ShowClientMessage(string message)
        {
            var safe = HttpUtility.JavaScriptStringEncode(message ?? string.Empty);
            var key = "alert_" + Guid.NewGuid().ToString("N");

            // Si usas UpdatePanel, puedes preferir ScriptManager.RegisterStartupScript
            if (ScriptManager.GetCurrent(this.Page) != null)
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), key, $"alert('{safe}');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(), key, $"alert('{safe}');", true);
            }
        }
    }
}
