using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class Conexion
    {

        public SqlConnection conexionBoxMolina()
        {
            SqlConnection conn = new SqlConnection();

            String connectionString
                = ConfigurationManager.ConnectionStrings["conexion_molina"].ConnectionString; ;

            conn.ConnectionString = connectionString;

            return conn;
        }
    }
}
