using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class ClienteDatos
    {
        private Conexion conexion = new Conexion();

        public Cliente login(Cliente clienteConsulta)
        {
            Cliente cliente = new Cliente();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT id_cliente,nombre_completo,CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('molina', contrasenna)) as contrasenna,cedula,confirmado,activo
  FROM Cliente where cedula = @cedula;
  ;", conexionSpartan);
            sqlCommand.Parameters.AddWithValue("@cedula", clienteConsulta.cedula);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                if (reader["contrasenna"].ToString().Equals(clienteConsulta.contrasenna))
                {
                    cliente.idCliente = Convert.ToInt32(reader["id_cliente"].ToString());
                    cliente.nombreCompleto = reader["nombre_completo"].ToString();
                    cliente.cedula = Convert.ToInt32(reader["cedula"].ToString());
                    cliente.confirmado = Convert.ToBoolean(reader["confirmado"].ToString());
                    cliente.activo = Convert.ToBoolean(reader["activo"].ToString());
                }
            }

            conexionSpartan.Close();

            return cliente;
        }

        public int insertarCliente(Cliente clienteInsertar)
        {
            Cliente cliente = new Cliente();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            String consulta = "insert Cliente (nombre_completo,telefono,correo,cedula,activo,confirmado,contrasenna,tipo_clase)"
+ "values(@nombreCompleto,@telefono,@correo,@cedula,@activo,@confirmado,ENCRYPTBYPASSPHRASE('molina', '" + clienteInsertar.contrasenna + "'),@tipoClase);"
  + "SELECT SCOPE_IDENTITY();";
            SqlCommand sqlCommand = new SqlCommand(consulta, conexionSpartan);

            sqlCommand.Parameters.AddWithValue("@nombreCompleto", clienteInsertar.nombreCompleto);
            sqlCommand.Parameters.AddWithValue("@telefono", clienteInsertar.telefono);
            sqlCommand.Parameters.AddWithValue("@correo", clienteInsertar.correo);
            sqlCommand.Parameters.AddWithValue("@cedula", clienteInsertar.cedula);
            sqlCommand.Parameters.AddWithValue("@activo", clienteInsertar.activo);
            sqlCommand.Parameters.AddWithValue("@confirmado", clienteInsertar.confirmado);
            sqlCommand.Parameters.AddWithValue("@tipoClase", clienteInsertar.tipoClase);

            conexionSpartan.Open();
            int idCliente = Convert.ToInt32(sqlCommand.ExecuteScalar());
            conexionSpartan.Close();

            return idCliente;
        }

        public List<Cliente> getClientes()
        {
            List<Cliente> listaClientes = new List<Cliente>();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT id_cliente,nombre_completo,CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('molina', contrasenna)) as contrasenna,cedula,confirmado,activo,telefono,correo,tipo_clase FROM Cliente order by nombre_completo;", conexionSpartan);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Cliente cliente = new Cliente();
                cliente.idCliente = Convert.ToInt32(reader["id_cliente"].ToString());
                cliente.nombreCompleto = reader["nombre_completo"].ToString();
                cliente.cedula = Convert.ToInt32(reader["cedula"].ToString());
                cliente.confirmado = Convert.ToBoolean(reader["confirmado"].ToString());
                cliente.activo = Convert.ToBoolean(reader["activo"].ToString());
                cliente.telefono = reader["telefono"].ToString();
                cliente.correo = reader["correo"].ToString();
                cliente.tipoClase = Convert.ToBoolean(reader["tipo_clase"].ToString());
                cliente.contrasenna = reader["contrasenna"].ToString();
                listaClientes.Add(cliente);
            }

            conexionSpartan.Close();

            return listaClientes;
        }

        public void actualizarCliente(Cliente cliente)
        {

            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"update Cliente set nombre_completo = @nombreCompleto, cedula = @cedula, confirmado = @confirmado,
                                                    activo = @activo, telefono = @telefono, correo = @correo, tipo_clase = @tipoClase, contrasenna = ENCRYPTBYPASSPHRASE('molina', '" + cliente.contrasenna + "') " +
                                                    "where id_cliente = @idCliente;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@nombreCompleto", cliente.nombreCompleto);
            sqlCommand.Parameters.AddWithValue("@telefono", cliente.telefono);
            sqlCommand.Parameters.AddWithValue("@correo", cliente.correo);
            sqlCommand.Parameters.AddWithValue("@cedula", cliente.cedula);
            sqlCommand.Parameters.AddWithValue("@activo", cliente.activo);
            sqlCommand.Parameters.AddWithValue("@confirmado", cliente.confirmado);
            sqlCommand.Parameters.AddWithValue("@tipoClase", cliente.tipoClase);
            sqlCommand.Parameters.AddWithValue("@idCliente", cliente.idCliente);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        public void eliminarCliente(Cliente cliente)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"delete Cliente where id_cliente = @idCliente;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idCliente", cliente.idCliente);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        public Cliente getClientePorCedula(Cliente clienteConsulta)
        {
            Cliente cliente = new Cliente();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT id_cliente,nombre_completo,telefono,CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('molina', contrasenna)) as contrasenna,cedula,confirmado,activo,correo,tipo_clase
  FROM Cliente where cedula = @cedula;
  ;", conexionSpartan);
            sqlCommand.Parameters.AddWithValue("@cedula", clienteConsulta.cedula);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                cliente.idCliente = Convert.ToInt32(reader["id_cliente"].ToString());
                cliente.nombreCompleto = reader["nombre_completo"].ToString();
                cliente.telefono = reader["telefono"].ToString();
                cliente.cedula = Convert.ToInt32(reader["cedula"].ToString());
                cliente.confirmado = Convert.ToBoolean(reader["confirmado"].ToString());
                cliente.activo = Convert.ToBoolean(reader["activo"].ToString());
                cliente.contrasenna = reader["contrasenna"].ToString();
                cliente.correo = reader["correo"].ToString();
                cliente.tipoClase = Convert.ToBoolean(reader["tipo_clase"].ToString());
            }

            conexionSpartan.Close();

            return cliente;
        }
    }
}
