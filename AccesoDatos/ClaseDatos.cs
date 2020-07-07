using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ClaseDatos
    {
        private Conexion conexion = new Conexion();

        public List<Clase> getClases()
        {
            List<Clase> listaClases = new List<Clase>();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM Clase order by hora;", conexionSpartan);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Clase clase = new Clase();
                clase.idClase = Convert.ToInt32(reader["id_Clase"].ToString());
                clase.hora = Convert.ToInt32(reader["hora"].ToString());
                clase.minutos = Convert.ToInt32(reader["minutos"].ToString());
                clase.cupo = Convert.ToInt32(reader["cupo"].ToString());
                listaClases.Add(clase);
            }

            conexionSpartan.Close();

            return listaClases;
        }

        public int insertarClase(Clase claseInsertar)
        {
            Cliente cliente = new Cliente();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            String consulta = @"insert Clase (cupo,hora,minutos) values(@cupo,@hora,@minutos); SELECT SCOPE_IDENTITY();";
            SqlCommand sqlCommand = new SqlCommand(consulta, conexionSpartan);

            sqlCommand.Parameters.AddWithValue("@cupo", claseInsertar.cupo);
            sqlCommand.Parameters.AddWithValue("@hora", claseInsertar.hora);
            sqlCommand.Parameters.AddWithValue("@minutos", claseInsertar.minutos);

            conexionSpartan.Open();
            int idClase = Convert.ToInt32(sqlCommand.ExecuteScalar());
            conexionSpartan.Close();

            return idClase;
        }

        public void eliminarClase(Clase clase)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"delete Clase where id_clase = @idClase;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idClase", clase.idClase);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        public void actualizarClase(Clase clase)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"update Clase set cupo = @cupo,hora = @hora,minutos = @minutos where id_clase = @idClase;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idClase", clase.idClase);
            sqlCommand.Parameters.AddWithValue("@cupo", clase.cupo);
            sqlCommand.Parameters.AddWithValue("@hora", clase.hora);
            sqlCommand.Parameters.AddWithValue("@minutos", clase.minutos);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }
    }
}
