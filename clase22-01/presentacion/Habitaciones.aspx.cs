using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class Habotaciones : System.Web.UI.Page
    {
        CNHabitaciones habitaciones = new CNHabitaciones();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect("index.aspx");
            }
            if (!IsPostBack)
            {
                CargaGrid();

            }
        }

        protected void CargaGrid()
        {
            dgvHabitaciones.DataSource = habitaciones.ObtenerhabitacionesN();
            dgvHabitaciones.DataBind();
        }

        protected void btnguardar_Click(object sender, EventArgs e)
        {
            int numero = Convert.ToInt32(txtnumero.Text);
            string descripcion = txtdescripcion.Text;
            int cant_huespedes = Convert.ToInt32(txtcantidad.Text);

            bool correcto = habitaciones.AgregarHabitaciones(numero, descripcion, cant_huespedes);
            if (correcto)
            {
                Response.Write("<script>alert('Habitación agregada correctamente');</script>");
                CargaGrid();
            }
            else
            {
                Response.Write("<script>alert('Error al agregar la habitación');</script>");
            }

        }

        


        protected void dgvHabitaciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(dgvHabitaciones.DataKeys[e.RowIndex].Value);

            if (habitaciones.EliminarHabitacion(id))
            {
                CargaGrid();
            }
        }


        protected void dgvHabitaciones_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvHabitaciones.EditIndex = e.NewEditIndex;
            CargaGrid();
        }

        protected void dgvHabitaciones_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgvHabitaciones.EditIndex = -1;
            CargaGrid();
        }

        protected void dgvHabitaciones_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(dgvHabitaciones.DataKeys[e.RowIndex].Value);
            GridViewRow row = dgvHabitaciones.Rows[e.RowIndex];
            int numero = int.Parse((row.Cells[1].Controls[0] as System.Web.UI.WebControls.TextBox).Text);
            string descripcion = (row.Cells[2].Controls[0] as System.Web.UI.WebControls.TextBox).Text;
            int cant_huespedes = int.Parse((row.Cells[3].Controls[0] as System.Web.UI.WebControls.TextBox).Text);

            if (habitaciones.ModificarHabitacion(id, numero, descripcion, cant_huespedes))
            {
                dgvHabitaciones.EditIndex = -1;
                CargaGrid();
            }
        }
    }
}