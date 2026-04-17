using HelpDesk.Security; // Igual que en userlogin: PasswordCrypto y sus constantes
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace HelpDesk
{
    public partial class agregarAgente : AdminOnlyPage
    {
        private readonly string strcon = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int insertedId;
                // Lee y normaliza entradas del formulario (IDs según tu .aspx)
                var email = (correo?.Text ?? string.Empty).Trim().ToLowerInvariant();
                var nombreAg = (nombre?.Text ?? string.Empty).Trim();
                var telefono = (contacto?.Text ?? string.Empty).Trim();
                var habilidades = (hbls?.Text ?? string.Empty).Trim(); // NVARCHAR(MAX)
                var pwd = (contrasena?.Text ?? string.Empty);
                var nivelStr = (listaSla?.SelectedValue ?? "0").Trim();

                // Validaciones mínimas
                if (string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(nombreAg) ||
                    string.IsNullOrWhiteSpace(pwd) ||
                    string.IsNullOrWhiteSpace(nivelStr))
                {
                    ShowClientMessage("Por favor completa correo, nombre, contraseña y nivel.");
                    return;
                }

                if (!int.TryParse(nivelStr, out var nivel) || nivel < 0)
                {
                    ShowClientMessage("Nivel inválido.");
                    return;
                }

                // === Genera salt + hash con el MISMO esquema de userlogin ===
                // Usamos el mismo helper/constantes que ya usas al verificar:
                // PasswordCrypto.VerifyPassword(pwd, dbSalt, dbHash) en userlogin.
                // Aquí creamos dbSalt/dbHash con PasswordCrypto para que coincida.
                byte[] salt = PasswordCrypto.CreateSalt();      // Debe producir 16 bytes (SaltSize)
                byte[] hash = PasswordCrypto.HashPassword(pwd, salt); // Debe producir 32 bytes (HashSize)

                // Asegura tamaños esperados (igual que haces al validar en userlogin)
                if (salt == null || hash == null ||
                    salt.Length != PasswordCrypto.SaltSize ||
                    hash.Length != PasswordCrypto.HashSize)
                {
                    ShowClientMessage("Error al generar las credenciales. Intenta de nuevo.");
                    return;
                }

                using (var con = new SqlConnection(strcon))
                {
                    con.Open();

                    // 1) Verificar que el email no exista (columna UNIQUE en hd.Agente)
                    using (var check = new SqlCommand(@"
                        SELECT 1
                        FROM hd.Agente
                        WHERE email = @Email;", con))
                    {
                        check.Parameters.Add("@Email", SqlDbType.NVarChar, 320).Value = email;
                        var exists = check.ExecuteScalar();
                        if (exists != null)
                        {
                            ShowClientMessage("El correo ya está registrado como agente.");
                            return;
                        }
                    }

                    // 2) Insertar el nuevo agente
                    using (var cmd = new SqlCommand(@"
                        INSERT INTO hd.Agente
                            (email, nombre, passwordHash, nivel, tAbiertos, telefono, habilidades, passwordSalt, Administrador)
                        OUTPUT INSERTED.agenteId
                        VALUES
                            (@Email, @Nombre, @PasswordHash, @Nivel, @TAbiertos, @Telefono, @Habilidades, @PasswordSalt, @Administrador);
                    ", con))
                    {
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 320).Value = email;
                        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 200).Value = nombreAg;
                        cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = hash;
                        cmd.Parameters.Add("@Nivel", SqlDbType.Int).Value = nivel;
                        cmd.Parameters.Add("@TAbiertos", SqlDbType.TinyInt).Value = 0; // inicia en 0
                        cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 25).Value =
                            (object)(string.IsNullOrWhiteSpace(telefono) ? null : telefono) ?? DBNull.Value;
                        // NVARCHAR(MAX) -> tamaño = -1
                        cmd.Parameters.Add("@Habilidades", SqlDbType.NVarChar, -1).Value =
                            (object)(string.IsNullOrWhiteSpace(habilidades) ? null : habilidades) ?? DBNull.Value;
                        cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, 16).Value = salt;
                        cmd.Parameters.Add("@Administrador", SqlDbType.Int).Value = chkAdministrador.Checked ? 1 : 0;

                        var insertedIdObj = cmd.ExecuteScalar();
                        insertedId = insertedIdObj == null ? 0 : Convert.ToInt32(insertedIdObj);

                        if (insertedId <= 0)
                        {
                            ShowClientMessage("No se pudo crear el agente.");
                            return;
                        }
                    }
                }


            

                ShowClientMessageAndRedirect(
                    $"Usuario {nombreAg} creado exitosamente. ID: {insertedId}",
                    "InicioAgente.aspx?registered=1"
                );

            contrasena.Text = string.Empty;
                habilidades = string.Empty;
                 correo.Text = nombre.Text = contacto.Text  = string.Empty;
                listaSla.ClearSelection();

                var slaItem = listaSla.Items.FindByValue("1");
                if (slaItem != null)
                {
                    listaSla.ClearSelection();
                    slaItem.Selected = true;
                }

            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // UNIQUE KEY violation
            {
                ShowClientMessage("El correo ya está registrado como agente.");
            }
            catch (Exception)
            {
                ShowClientMessage("Ocurrió un error al crear el agente.");
            }
        }

    }
}