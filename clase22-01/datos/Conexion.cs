using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datos
{
    public class Conexion
    {
        private static string cadena = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=loginDB;Integrated Security=True";

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection cn = new SqlConnection(cadena);
            return cn;
        }
    }
}
