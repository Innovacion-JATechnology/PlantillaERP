using HelpDesk.Security;  
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HelpDesk
{
    public partial class usersignup : AgentOnlyPage
    {
        private readonly string strcon = ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                fillEmpresa();
        }

        // Botón Registrar
        protected void Button1_Click(object sender, EventArgs e)
        {
            // Primero valida si el correo ya existe
            if (checkMemberExists())
            {
                ShowClientMessage("El correo ya existe en la base de datos.");
                return;
            }

            signUpNewUser();
        }
         
        /// Verifica si el correo ya existe (consulta parametrizada).
        private bool checkMemberExists()
        {
            try
            {
                var email = (correo.Text ?? string.Empty).Trim().ToLowerInvariant();

                using (var con = new SqlConnection(strcon))
                using (var cmd = new SqlCommand("SELECT 1 FROM hd.Usuario WHERE Correo = @Correo;", con))
                {
                    cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 256).Value = email;

                    con.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
            catch (Exception)
            {                
                ShowClientMessage("Error validando el correo.");// No exponemos detalles técnicos
                return true; // por seguridad, evita crear si falla la validación
            }
        }
         
        /// Inserta un nuevo usuario usando PasswordCrypto para hash/salt. 
        private void signUpNewUser()
        {
            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(nombre.Text))
            {
                ShowClientMessage("El nombre es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(correo.Text))
            {
                ShowClientMessage("El correo es obligatorio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(contrasena.Text))
            {
                ShowClientMessage("Escriba una contraseña.");
                return;
            }

            if (string.IsNullOrEmpty(listaempresa.SelectedValue)) 
            { 
                ShowClientMessage("Seleccione empresa."); 
                return;
            }
            
            if (string.IsNullOrEmpty(listaSla.SelectedValue))      
            { 
                ShowClientMessage("Seleccione nivel de servicio (SLA)."); 
                return; 
            }

            // Normaliza correo
            var email = correo.Text.Trim().ToLowerInvariant();

            // Parseos seguros de IDs opcionales (si no hay selección -> NULL)
            int? empresaId = null;
            int empParsed;
            if (int.TryParse(listaempresa.SelectedValue, out empParsed))
                empresaId = empParsed;

            int? slaId = null;
            int slaParsed;
            if (int.TryParse(listaSla.SelectedValue, out slaParsed))
                slaId = slaParsed;

            int? puestoId = null;
            int puestoParsed;
            if (int.TryParse(listaPuesto.SelectedValue, out puestoParsed))
                puestoId = puestoParsed;

          

            // Hashing de contraseña (usa tu namespace criptográfico)
            var salt = PasswordCrypto.CreateSalt();
            var hash = PasswordCrypto.HashPassword(contrasena.Text.Trim(), salt);

            try
            {

                int newID;
                using (var con = new SqlConnection(strcon))
                using (var cmd = new SqlCommand(@"
                    INSERT INTO hd.Usuario
                    (Nombre, ApPaterno, ApMaterno, Correo, Contacto, Empresa, Puesto, SLA, Activo,
                     CreadoEn, CreadoPor, ActualizadoEn, ActualizadoPor, PasswordHash, Estatus, PasswordSalt)
                    VALUES
                    (@Nombre, @ApPaterno, @ApMaterno, @Correo, @Contacto, @Empresa, @Puesto, @SLA, @Activo,
                     @CreadoEn, @CreadoPor, @ActualizadoEn, @ActualizadoPor, @PasswordHash, @Estatus, @PasswordSalt);
                ", con))
                {
                    // Requeridos
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = nombre.Text.Trim();
                    cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 256).Value = email;

                    // Opcionales NVARCHAR
                    if (string.IsNullOrWhiteSpace(paterno.Text))
                        cmd.Parameters.Add("@ApPaterno", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@ApPaterno", SqlDbType.NVarChar, 50).Value = paterno.Text.Trim();

                    if (string.IsNullOrWhiteSpace(materno.Text))
                        cmd.Parameters.Add("@ApMaterno", SqlDbType.NVarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@ApMaterno", SqlDbType.NVarChar, 50).Value = materno.Text.Trim();

                    if (string.IsNullOrWhiteSpace(contacto.Text))
                        cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 25).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 25).Value = contacto.Text.Trim();


                    cmd.Parameters.Add("@Empresa", SqlDbType.Int).Value =
                        string.IsNullOrEmpty(listaempresa.SelectedValue) ? (object)DBNull.Value : int.Parse(listaempresa.SelectedValue);


                    cmd.Parameters.Add("@Puesto", SqlDbType.Int).Value =
                      string.IsNullOrEmpty(listaPuesto.SelectedValue) ? (object)DBNull.Value : int.Parse(listaPuesto.SelectedValue);


                    cmd.Parameters.Add("@SLA", SqlDbType.Int).Value =
                        string.IsNullOrEmpty(listaSla.SelectedValue) ? (object)DBNull.Value : int.Parse(listaSla.SelectedValue);


                    // Flags obligatorias
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("@Estatus", SqlDbType.Bit).Value = true;

                    // Auditoría
                    cmd.Parameters.Add("@CreadoEn", SqlDbType.DateTime2).Value = DateTime.UtcNow;
                 
                   
                    object creadoPor = Session["agentid"] ?? (object)DBNull.Value;
                    if (creadoPor is string s && long.TryParse(s, out var creadoPorId))
                        cmd.Parameters.Add("@CreadoPor", SqlDbType.BigInt).Value = creadoPorId;
                    else
                        cmd.Parameters.Add("@CreadoPor", SqlDbType.BigInt).Value = DBNull.Value;


                    cmd.Parameters.Add("@ActualizadoEn", SqlDbType.DateTime2).Value = DBNull.Value;
                    cmd.Parameters.Add("@ActualizadoPor", SqlDbType.BigInt).Value = DBNull.Value;

                    // Credenciales seguras (usa tamaños del PasswordCrypto)
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, PasswordCrypto.HashSize).Value = hash; // 32 bytes
                    cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, PasswordCrypto.SaltSize).Value = salt; // 16 bytes

                    con.Open();
                   // cmd.ExecuteNonQuery();

                    object result = cmd.ExecuteScalar();
                    newID = Convert.ToInt32(result);
                }

                ShowClientMessageAndRedirect(
                    $"Usuario {nombre.Text} creado exitosamente. ID: {newID}",
                    "InicioAgente.aspx?registered=1"
                );

            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // unique violation
            {
                ShowClientMessage("El correo ya fue registrado.");
            }
            catch (Exception)
            {
                ShowClientMessage("Ocurrió un error al registrar el usuario.");
                // TODO: log de la excepción
            }
        }

        /// <summary>
        /// Carga empresas y SLAs 
        /// </summary>
        private void fillEmpresa()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT EmpresaId, Nombre FROM hd.empresa ORDER BY nombre;", con))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        listaempresa.DataSource = dt;
                        listaempresa.DataValueField = "EmpresaId";
                        listaempresa.DataTextField = "Nombre";
                        listaempresa.DataBind();
                    }

                    listaempresa.Items.Insert(0, new ListItem("Seleccione empresa", ""));

                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT SLAId, Nombre FROM hd.SLA ORDER BY SLAid;", con))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        listaSla.DataSource = dt;
                        listaSla.DataValueField = "SLAId";
                        listaSla.DataTextField = "Nombre";
                        listaSla.DataBind();
                    }

                    listaSla.Items.Insert(0, new ListItem("Sel. Nivel de Servicio", ""));


                    using (SqlCommand cmd = new SqlCommand(
                      "SELECT puestoId, Nombre FROM hd.Puesto;", con))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        listaPuesto.DataSource = dt;
                        listaPuesto.DataValueField = "puestoId";
                        listaPuesto.DataTextField = "Nombre";
                        listaPuesto.DataBind();
                    }

                    listaPuesto.Items.Insert(0, new ListItem("Seleccione Puesto", ""));
                }
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(), "err", "alert('Error al cargar empresas');", true);
            }
        }

       
    }
}