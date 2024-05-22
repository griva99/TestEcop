using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TestEcop.Clases
{
    public class Pedido
    {
        public int id { get; set; }
        public float preciototal { get; set; }
        public int cantidad { get; set; }
        public List<DetallePedido> detalle { get; set; }
    }
    public class DetallePedido
    {
        public int id { get; set; }
        public int id_pedido { get; set; }
        public int id_producto { get; set; }
        public int id_cliente { get; set; }
        public float precio { get; set; }
        public float preciototal { get; set; }
    }
    public class VistaDetalle
    {
        public int id_producto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public float precio { get; set; }
    }
    public class PedidoDDL
    {
        ConexionDB conexionDB { get; set; } = new ConexionDB();
        protected DataTable GetProducto()
        {
            DataTable dt = new DataTable();
            string query = "SELECT id, descripcion FROM productos";
            string connectionString = conexionDB.ConexionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader); // Carga los datos del lector en el DataTable
            }

            return dt;
        }

        public void DropDownListProducto(DropDownList e)
        {
            e.Items.Clear();
            e.Items.Add(new ListItem("Elige los productos", "0"));
            e.AppendDataBoundItems = true;
            e.DataSource = GetProducto();
            e.DataTextField = "descripcion";
            e.DataValueField = "id";
            e.DataBind();
        }
        protected DataTable GetCliente()
        {
            DataTable dt = new DataTable();
            string query = "SELECT c.id, c.nombre + ' ' + c.apellido AS nombre FROM [clientes] AS c;";
            string connectionString = conexionDB.ConexionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader); // Carga los datos del lector en el DataTable
            }

            return dt;
        }

        public void DropDownListCliente(DropDownList e)
        {
            e.Items.Clear();
            e.Items.Add(new ListItem("Elige el cliente", "0"));
            e.AppendDataBoundItems = true;
            e.DataSource = GetCliente();
            e.DataTextField = "nombre";
            e.DataValueField = "id";
            e.DataBind();
        }
    }
    public class PedidoVista
    {
        ConexionDB conexionDB { get; set; } = new ConexionDB();
        public List<VistaDetalle> GetVista(int idcliente, int idproducto)
        {
            string connectionString = conexionDB.ConexionString();
            List<VistaDetalle> nuevosProductos = new List<VistaDetalle>();  // Lista para almacenar los nuevos productos

            string query = "SELECT c.nombre + ' ' + c.apellido AS nombre, p.descripcion, p.precio FROM [clientes] as c " +
               $"CROSS JOIN [productos] as p where p.id = {idproducto} and c.id={idcliente}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    VistaDetalle nuevoProducto = new VistaDetalle
                    {
                        nombre = reader.GetString(0),
                        descripcion = reader.GetString(1),
                        precio = (float)reader.GetDouble(2)
                    };

                    nuevosProductos.Add(nuevoProducto);  // Agrega el nuevo producto a la lista de nuevos productos
                }

                reader.Close();
            }

            return nuevosProductos;  // Devuelve solo los nuevos productos obtenidos en esta llamada
        }
        public int guardar(Pedido pedido)
        {
            int idPedido = 0;
            string connectionString = conexionDB.ConexionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insertar el pedido
                    string queryPedido = "INSERT INTO pedidos (preciototal, cantidad, fecha) OUTPUT INSERTED.id VALUES (@preciototal, @cantidad, GETDATE())";

                    using (SqlCommand cmdPedido = new SqlCommand(queryPedido, connection, transaction))
                    {
                        cmdPedido.Parameters.AddWithValue("@preciototal", pedido.preciototal);
                        cmdPedido.Parameters.AddWithValue("@cantidad", pedido.cantidad);

                        idPedido = (int)cmdPedido.ExecuteScalar();
                    }

                    // Insertar los detalles del pedido
                    string queryDetalle = "INSERT INTO detallepedido (id_pedido, id_producto, id_cliente,precio,preciototal) VALUES (@id_pedido, @id_producto, @id_cliente,@precio,@preciototal)";

                    foreach (var detalle in pedido.detalle)
                    {
                        using (SqlCommand cmdDetalle = new SqlCommand(queryDetalle, connection, transaction))
                        {
                            cmdDetalle.Parameters.AddWithValue("@id_pedido", idPedido);
                            cmdDetalle.Parameters.AddWithValue("@id_producto", detalle.id_producto);
                            cmdDetalle.Parameters.AddWithValue("@id_cliente", detalle.id_cliente);
                            cmdDetalle.Parameters.AddWithValue("@precio", detalle.precio);
                            cmdDetalle.Parameters.AddWithValue("@preciototal", detalle.preciototal);

                            cmdDetalle.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Error al insertar el pedido y sus detalles: " + ex.Message);
                    return -1;
                }
            }

            return idPedido;
        }
    }
}