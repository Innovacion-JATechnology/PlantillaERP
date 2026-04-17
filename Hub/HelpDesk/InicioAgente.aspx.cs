using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HelpDesk
{
    public partial class InicioAgente : AgentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var role = Session["role"] as string;

                if (string.Equals(role, "agente", StringComparison.OrdinalIgnoreCase))
                {
                    // Hide admin-only options 
                    divVerAgentes.Visible = false;
                    divAgregarAgente.Visible = false; 
                }
            }

        }
    }
}