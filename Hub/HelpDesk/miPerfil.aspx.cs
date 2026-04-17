using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient; 

namespace HelpDesk
{
    public partial class miPerfil : UsuarioOnlyPage 
    {
        private readonly string strcon =
            ConfigurationManager.ConnectionStrings["ServerCon"].ConnectionString;
        Dictionary<int, string> map;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    FillCombos();   // Empresas + SLA
                    LoadProfile();  // Datos del usuario
                }
                catch (Exception)
                {
                    ShowClientMessage("Ocurrió un error al cargar el perfil.");
                }
            }
        }

        // Obtiene el id de usuario desde Session 
        private int? CurrentUserId
        {
            get
            {
                var obj = Session["userid"];
                if (obj == null) return null;
                if (obj is int i) return i;
                if (int.TryParse(obj.ToString(), out var id)) return id;
                return null;
            }
        }

        private void FillCombos()
        {
            map = new Dictionary<int, string>();
            using (var con = new SqlConnection(strcon))
            {
                con.Open();

                // Empresas
                using (var cmd = new SqlCommand(
                    "SELECT EmpresaId, Nombre FROM hd.Empresa ORDER BY Nombre;", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["EmpresaId"]);
                        string nombre = row["Nombre"].ToString();
                        map[id] = nombre;
                    }
                }




            }
        }

        private void LoadProfile()
        {
            if (CurrentUserId == null)
            {
                ShowClientMessage("No hay sesión de usuario.");
                return;
            }

            using (var con = new SqlConnection(strcon))
            using (var cmd = new SqlCommand(@"
                SELECT  u.UsuarioId, u.Nombre, u.ApPaterno, u.ApMaterno,
                        u.Correo, u.Contacto, u.Empresa, u.Puesto, u.SLA,
                        u.Activo, u.Estatus,
                        p.Nombre AS PuestoNombre
                FROM hd.Usuario u
                LEFT JOIN hd.Puesto p ON p.PuestoId = u.Puesto
                WHERE u.UsuarioId = @UserId;", con))
            {
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = CurrentUserId.Value;
                con.Open();

                using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!rdr.Read())
                    {
                        ShowClientMessage("No se encontró el perfil de usuario.");
                        return;
                    }

                    // ID (solo lectura para mostrar)
                    // Datos personales
                    TextBox1.Text = rdr["UsuarioId"]?.ToString();
                    string nm = rdr["Nombre"] as string ?? string.Empty;
                    string apP = rdr["ApPaterno"] as string ?? string.Empty;
                    string apM = rdr["ApMaterno"] as string ?? string.Empty;

                    TextBox3.Text = nm + " " + apP + " " + apM; 

                    // Contacto
                    TextBox5.Text = rdr["Correo"] as string ?? string.Empty;
                    TextBox6.Text = rdr["Contacto"] as string ?? string.Empty;

                    // Puesto (solo mostramos nombre del puesto en TextBox8)
                    TextBox8.Text = rdr["PuestoNombre"] as string ?? string.Empty;



                    int empresaId = rdr["Empresa"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["Empresa"]);

                    if (map.ContainsKey(empresaId))
                        TextBox4.Text = map[empresaId];
                    else
                        TextBox4.Text = "";



                    // Estatus visual
                    var activo = rdr["Activo"] != DBNull.Value && Convert.ToBoolean(rdr["Activo"]);
                    var estatus = rdr["Estatus"] != DBNull.Value && Convert.ToBoolean(rdr["Estatus"]);
                    SetStatusBadge(activo && estatus);
                }
            }
        }

        private static void SelectByValueSafe(DropDownList ddl, string value)
        {
            if (ddl == null) return;
            if (string.IsNullOrEmpty(value))
            {
                ddl.ClearSelection();
                return;
            }

            var item = ddl.Items.FindByValue(value);
            if (item != null)
            {
                ddl.ClearSelection();
                item.Selected = true;
            }
        }

        private void SetStatusBadge(bool activo)
        {
            // Usa CssClass en el .aspx para el Label si puedes
            Label1.Text = activo ? "Activo" : "Inactivo";
            Label1.CssClass = activo
                ? "badge badge-pill badge-success"
                : "badge badge-pill badge-secondary";
        }

        private void ShowClientMessage(string message)
        {
            // Muestra un alert sin romper comillas
            var safe = (message ?? string.Empty).Replace("'", "\\'");
            ClientScript.RegisterStartupScript(
                this.GetType(), "msg", $"alert('{safe}');", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}