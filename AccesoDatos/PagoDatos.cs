using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class PagoDatos
    {
        private Conexion conexion = new Conexion();

        public List<Pago> getPagos()
        {
            List<Pago> listaPagos = new List<Pago>();
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT P.*,C.descripcion, C.monto as monto_costo,CL.nombre_completo,CL.activo,CL.cedula,
CL.confirmado, CL.correo, CL.telefono, CL.tipo_clase
FROM Pago P, Costo C,Cliente CL
Where C.id_costo = P.id_costo and CL.id_cliente = P.id_cliente;", conexionSpartan);
            SqlDataReader reader;

            conexionSpartan.Open();
            reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Pago pago = new Pago();
                Costo costo = new Costo();
                Cliente cliente = new Cliente();

                cliente.activo = Convert.ToBoolean(reader["activo"].ToString());
                cliente.cedula = Convert.ToInt32(reader["cedula"].ToString());
                cliente.confirmado = Convert.ToBoolean(reader["confirmado"].ToString());
                cliente.correo = reader["correo"].ToString();
                cliente.idCliente = Convert.ToInt32(reader["id_cliente"].ToString());
                cliente.nombreCompleto = reader["nombre_completo"].ToString();
                cliente.telefono = reader["telefono"].ToString();
                cliente.tipoClase = Convert.ToBoolean(reader["tipo_clase"].ToString());

                costo.descripcion = reader["descripcion"].ToString();
                costo.idCosto = Convert.ToInt32(reader["id_costo"].ToString());
                costo.monto = Convert.ToDouble(reader["monto_costo"].ToString());

                pago.fechaDesde = Convert.ToDateTime(reader["fecha_desde"].ToString());
                pago.fechaHasta = Convert.ToDateTime(reader["fecha_hasta"].ToString());
                pago.monto = Convert.ToDouble(reader["monto"].ToString());
                pago.idPago = Convert.ToInt32(reader["id_pago"].ToString());

                pago.costo = costo;
                pago.cliente = cliente;

                listaPagos.Add(pago);
            }

            conexionSpartan.Close();

            return listaPagos;
        }

        public int insertarPago(Pago pagoInsertar)
        {
            SqlConnection conexionSpartan = conexion.conexionBoxMolina();
            String consulta = @"insert Pago (id_costo,monto,fecha_desde,fecha_hasta,id_cliente)
                                                values(@idCosto,@monto,@fechaDesde,@fechaHasta,@idCliente); SELECT SCOPE_IDENTITY();";
            SqlCommand sqlCommand = new SqlCommand(consulta, conexionSpartan);

            sqlCommand.Parameters.AddWithValue("@idCosto", pagoInsertar.costo.idCosto);
            sqlCommand.Parameters.AddWithValue("@monto", pagoInsertar.monto);
            sqlCommand.Parameters.AddWithValue("@fechaDesde", pagoInsertar.fechaDesde);
            sqlCommand.Parameters.AddWithValue("@fechaHasta", pagoInsertar.fechaHasta);
            sqlCommand.Parameters.AddWithValue("@idCliente", pagoInsertar.cliente.idCliente);

            conexionSpartan.Open();
            int idPago = Convert.ToInt32(sqlCommand.ExecuteScalar());
            conexionSpartan.Close();

            return idPago;
        }

        public void eliminarPago(Pago pago)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"delete Pago where id_pago = @idPago;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idPago", pago.idPago);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }

        public void actualizarPago(Pago pago)
        {
            SqlConnection sqlConnection = conexion.conexionBoxMolina();

            SqlCommand sqlCommand = new SqlCommand(@"update Pago set id_costo = @idCosto,monto = @monto,fecha_desde = @fechaDesde,
fecha_hasta = @fechaHasta,id_cliente = @idCliente where id_pago = @idPago;", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@idPago", pago.idPago);
            sqlCommand.Parameters.AddWithValue("@idCosto", pago.costo.idCosto);
            sqlCommand.Parameters.AddWithValue("@monto", pago.monto);
            sqlCommand.Parameters.AddWithValue("@fechaDesde", pago.fechaDesde);
            sqlCommand.Parameters.AddWithValue("@fechaHasta", pago.fechaHasta);
            sqlCommand.Parameters.AddWithValue("@idCliente", pago.cliente.idCliente);

            sqlConnection.Open();
            sqlCommand.ExecuteReader();

            sqlConnection.Close();
        }
    }
}
