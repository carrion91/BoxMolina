using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class AdministradorDatos
    {
        private Conexion conexion = new Conexion();

        public Boolean login(Administrador administradorConsulta)
        {
            Boolean loginExitoso = false;
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT id_administrador,usuario,CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('molina', contrasenna)) as contrasenna
  FROM Administrador where usuario = @usuario;
  ;", conexionSpartan);
            sqlCommand.Parameters.AddWithValue("@usuario", administradorConsulta.usuario);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                if(reader["contrasenna"].ToString().Equals(administradorConsulta.contrasenna))
                loginExitoso = true;
            }

            conexionSpartan.Close();

            return loginExitoso;
        }
    }
}
