using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class CostoDatos
    {
        private Conexion conexion = new Conexion();

        public List<Costo> getCostos()
        {
            List<Costo> listaCostos = new List<Costo>();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT * FROM Costo order by descripcion;", conexionSpartan);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Costo costo = new Costo();
                costo.idCosto = Convert.ToInt32(reader["id_Costo"].ToString());
                costo.descripcion = reader["descripcion"].ToString();
                costo.monto = Convert.ToDouble(reader["monto"].ToString());
                listaCostos.Add(costo);
            }

            conexionSpartan.Close();

            return listaCostos;
        }

        public int insertarCosto(Costo costoInsertar)
        {
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            String consulta = @"insert costo (descripcion,monto) values(@descripcion,@monto); SELECT SCOPE_IDENTITY();";
            SqlCommand sqlCommand = new SqlCommand(consulta, conexionSpartan);

            sqlCommand.Parameters.AddWithValue("@descripcion", costoInsertar.descripcion);
            sqlCommand.Parameters.AddWithValue("@monto", costoInsertar.monto);

            conexionSpartan.Open();
            int idCosto = Convert.ToInt32(sqlCommand.ExecuteScalar());
            conexionSpartan.Close();

            return idCosto;
        }

        public void eliminarCosto(Costo costo)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"delete costo where id_costo = @idCosto;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idCosto", costo.idCosto);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        public void actualizarCosto(Costo costo)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"update costo set descripcion = @descripcion,monto = @monto where id_costo = @idCosto;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idCosto", costo.idCosto);
            sqlCommand.Parameters.AddWithValue("@descripcion", costo.descripcion);
            sqlCommand.Parameters.AddWithValue("@monto", costo.monto);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }
    }
}
