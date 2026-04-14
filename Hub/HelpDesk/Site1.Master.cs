using System;
using System.Web.UI.WebControls;

namespace HelpDesk
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                SetInicioLinkByRole();
            }

            try
            {
                var role = Session["role"] as string;
                if (string.IsNullOrEmpty(role))
                {
                    linkIngreso.Visible = true;     // user login link button
                    linkSalir.Visible = false;      // logout link button
                    linkHola.Visible = false;       // hello user link button

                    linkAdministrador.Visible = true; // admin login link button
                    linkMantenimiento.Visible = false;
                    linkCatalogo.Visible = false; // member management link button
                    linkHistorial.Visible = false; // historial link button
                    linkRegistro.Visible = false; // regsitro link button
                    AgregarAgente.Visible = false;
                }
                else if (Session["role"].Equals("usuario"))
                {
                    linkIngreso.Visible = false;     // user login link button
                    linkSalir.Visible = true;      // logout link button
                    linkHola.Visible = true;       // hello user link button
                    linkHola.Text = "Usuario: " + Session["fullname"];

                    linkAdministrador.Visible = true; // admin login link button
                    linkMantenimiento.Visible = false;
                    linkCatalogo.Visible = false; // member management link button
                    linkHistorial.Visible = false; // hirtorial link button
                    linkRegistro.Visible = false; // reistro link button
                    AgregarAgente.Visible = false;

                }
                else if (Session["role"].Equals("agente"))
                {
                    linkIngreso.Visible = false;     // user login link button
                    linkSalir.Visible = true;      // logout link button
                    linkHola.Visible = true;       // hello user link button
                    linkHola.Text = "Agente: " + Session["fullname"];

                    linkAdministrador.Visible = false; // admin login link button
                    linkMantenimiento.Visible = true;
                    linkCatalogo.Visible = true; // member management link button
                    linkHistorial.Visible = true; // ticket management link button
                    linkRegistro.Visible = true; // registro link button
                    AgregarAgente.Visible = false;
                }
                else if (Session["role"].Equals("admin"))
                {
                    linkIngreso.Visible = false;     // user login link button
                    linkSalir.Visible = true;      // logout link button
                    linkHola.Visible = true;       // hello user link button
                    linkHola.Text = "Admin: " + Session["fullname"];

                    linkAdministrador.Visible = false; // admin login link button
                    linkMantenimiento.Visible = true;
                    linkCatalogo.Visible = true; // member management link button
                    linkHistorial.Visible = true; // ticket management link button
                    linkRegistro.Visible = true; // registro link button
                    AgregarAgente.Visible = true;  // Solo admins pueden agregar agentes
                }
            }
            catch (Exception ex)
            {
            }


        }

        private void SetInicioLinkByRole()
        {

            var role = (Session["role"] ?? string.Empty).ToString().Trim().ToLowerInvariant();

            string targetUrl;

            switch (role)
            {
                case "":
                case null:
                    targetUrl = "~/homepage.aspx";              // No autenticado
                    break;

                case "usuario":
                    targetUrl = "~/InicioUsuario.aspx";       // Usuario
                    break;

                case "agente":
                case "admin":
                    targetUrl = "~/InicioAgente.aspx";        // Agente / Admin
                    break;

                default:
                    targetUrl = "~/homepage.aspx";
                    break;
            }

            lnkInicio.HRef = ResolveUrl(targetUrl);

        }



        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminlogin.aspx");
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {

            Response.Redirect("ActualizaTablas.aspx");
        }

        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("adminAllTickets.aspx");
        }

        protected void LinkButton12_Click(object sender, EventArgs e)
        {

                 Response.Redirect("allUsers.aspx");
        }

        protected void LinkButton10_Click(object sender, EventArgs e)
        {

            Response.Redirect("usersignup.aspx");
        }

        protected void LinkButton18_Click(object sender, EventArgs e)
        {

            Response.Redirect("agregarAgente.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlogin.aspx");
        }

        protected void linkSalir_Click(object sender, EventArgs e)
        { 
            linkIngreso.Visible = true;     // user login link button
            linkSalir.Visible = false;      // logout link button
            linkHola.Visible = false;       // hello user link button

            linkAdministrador.Visible = true; // admin login link button
            linkMantenimiento.Visible = false;
            linkCatalogo.Visible = false; // member management link button
            linkHistorial.Visible = false; // historial link button
            linkRegistro.Visible = false; // regsitro link button
             
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/homepage.aspx", false);

        }

        protected void linkHola_Click(object sender, EventArgs e)
        {

            Response.Redirect("miPerfil.aspx", false);
        }

        
    }
}