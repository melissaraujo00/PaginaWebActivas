using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datos
{
    public class CDPersonas
    {
        public bool ValidarUsuario(string usuario, string clave)
        {
            bool existe = false;

            using (SqlConnection cn = Conexion.ObtenerConexion())
            {
                string sql = "SELECT COUNT(*) FROM DBUsuarios WHERE usuario=@u AND clave=@c";
                SqlCommand cmd = new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@c", clave);

                cn.Open();

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                    existe = true;

            }
            return existe;
        }
    }
}
