using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class index : System.Web.UI.Page
    {
        CNPersonas bll = new CNPersonas();
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            bool acceso = bll.Login(txtUsuario.Text, password.Text);

            if (acceso)
            {
                Response.Redirect("principal.aspx");
            }
            else
            {
                lblmensaje.Text = "Acceso denegado";
            }

        }
    }
}