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
    public class VistaDetalle
    {
        public int id { get; set; }
        public int id_producto { get; set; }
        public int id_cliente { get; set; }
        public string descripcion { get; set; }
        public string nombre { get; set; }
        public float precio { get; set; }
        public float preciototal { get; set; }
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
        public int detalleTemp(VistaDetalle vista)
        {
            int nro = 0;
            float preciototal = 0;
            string connectionString = conexionDB.ConexionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string querySumaPrecio = "SELECT ISNULL(SUM(precio), 0) FROM [temp_detalle]";
                SqlCommand cmdSuma = new SqlCommand(querySumaPrecio, connection);
                connection.Open();
                object result = cmdSuma.ExecuteScalar();
                decimal sumaPrecios = 0;

                if (result != null && result != DBNull.Value)
                {
                    sumaPrecios = Convert.ToDecimal(result);
                }

                preciototal = (float)sumaPrecios + vista.precio; // Suma el precio actual
                vista.preciototal = preciototal;
            }

            // Paso 2: Insertar el nuevo registro con el preciototal calculado
            string sqlQuery = "INSERT INTO temp_detalle (id_producto, id_cliente, nombre, descripcion, precio, preciototal) " +
                              "VALUES (@id_producto, @id_cliente, @nombre, @descripcion, @precio, @preciototal)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@id_producto", vista.id_producto);
                        sc.Parameters.AddWithValue("@id_cliente", vista.id_cliente);
                        sc.Parameters.AddWithValue("@nombre", vista.nombre);
                        sc.Parameters.AddWithValue("@descripcion", vista.descripcion);
                        sc.Parameters.AddWithValue("@precio", vista.precio);
                        sc.Parameters.AddWithValue("@preciototal", vista.preciototal);
                        nro = sc.ExecuteNonQuery();
                    }

                    // Paso 3: Actualizar el preciototal en todos los registros
                    string updateQuery = "UPDATE temp_detalle SET preciototal = @preciototal";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con))
                    {
                        cmdUpdate.Parameters.AddWithValue("@preciototal", preciototal);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra el mensaje de error.
                Console.WriteLine("Error al insertar en la base de datos: " + ex.Message);
            }

            return nro;
        }
        public float GetTotal()
        {
            string connectionString = conexionDB.ConexionString();
            float total = 0; // Inicializar total con 0 por si no se obtienen resultados

            string query = "SELECT preciototal FROM [temp_detalle]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    total = Convert.ToSingle(result);
                }
            }

            return total;  // Devuelve la suma de preciototal
        }
        public int EliminarTemp()
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "DELETE FROM [temp_detalle]";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        nro = sc.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra el mensaje de error.
                Console.WriteLine("Error al eliminar en la base de datos: " + ex.Message);
            }
            return nro;
        }

        public int Eliminar(int id)
        {
            int nro = 0;
            float preciototal = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQueryDelete = "DELETE FROM [temp_detalle] WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQueryDelete, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@id", id);
                        nro = sc.ExecuteNonQuery();
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string querySumaPrecio = "SELECT ISNULL(SUM(precio), 0) FROM [temp_detalle]";
                        SqlCommand cmdSuma = new SqlCommand(querySumaPrecio, connection);
                        connection.Open();
                        object result = cmdSuma.ExecuteScalar();
                        decimal sumaPrecios = 0;

                        if (result != null && result != DBNull.Value)
                        {
                            sumaPrecios = Convert.ToDecimal(result);
                        }

                        preciototal = (float)sumaPrecios; // Suma el precio actual
                    }
                    // Paso 3: Actualizar el preciototal en todos los registros
                    string updateQuery = "UPDATE temp_detalle SET preciototal = @preciototal";
                    using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, con))
                    {
                        cmdUpdate.Parameters.AddWithValue("@preciototal", preciototal);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra el mensaje de error.
                Console.WriteLine("Error al eliminar en la base de datos: " + ex.Message);
            }

            return nro;
        }

        public VistaDetalle GetVista(int idcliente, int idproducto)
        {
            string connectionString = conexionDB.ConexionString();
            VistaDetalle nuevoProducto = null;  // Variable para almacenar el nuevo producto

            string query = "SELECT c.nombre + ' ' + c.apellido AS nombre, p.descripcion, p.precio FROM [clientes] as c " +
               $"CROSS JOIN [productos] as p where p.id = {idproducto} and c.id={idcliente}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) // Usamos if en lugar de while porque esperamos solo un resultado
                {
                    nuevoProducto = new VistaDetalle
                    {
                        nombre = reader.GetString(0),
                        descripcion = reader.GetString(1),
                        precio = (float)reader.GetDouble(2)
                    };
                }

                reader.Close();
            }

            return nuevoProducto;  // Devuelve el nuevo producto obtenido en esta llamada
        }
        public List<VistaDetalle> GetGrilla()
        {

            string connectionString = conexionDB.ConexionString();
            List<VistaDetalle> grillas = new List<VistaDetalle>();

            string query = "select id,id_producto, id_cliente, nombre, descripcion, precio, preciototal from [temp_detalle]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    VistaDetalle grilla = new VistaDetalle
                    {
                        id = reader.GetInt32(0),
                        id_producto = reader.GetInt32(1),
                        id_cliente = reader.GetInt32(2),
                        nombre = reader.GetString(3),
                        descripcion = reader.GetString(4),
                        precio = (float)reader.GetDouble(5),
                        preciototal = (float)reader.GetDouble(6)
                    };

                    grillas.Add(grilla);
                }

                reader.Close();
            }

            return grillas;
        }
        public int guardar(float precioTotal,int cantidad)
        {
            int nro = 0;
            int idPedido = 0;
            string connectionString = conexionDB.ConexionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Paso 1: Insertar el pedido y obtener el ID generado
                    string queryPedido = "INSERT INTO pedidos (preciototal, cantidad, fecha) OUTPUT INSERTED.id VALUES (@preciototal, @cantidad, GETDATE())";

                    using (SqlCommand cmdPedido = new SqlCommand(queryPedido, connection, transaction))
                    {
                        cmdPedido.Parameters.AddWithValue("@preciototal", precioTotal);
                        cmdPedido.Parameters.AddWithValue("@cantidad", cantidad);

                        idPedido = (int)cmdPedido.ExecuteScalar();
                    }

                    // Paso 2: Insertar los detalles del pedido desde detalle_temp
                    string queryDetalle = @"
                INSERT INTO detallepedido (id_pedido, id_producto, id_cliente, precio, preciototal)
                SELECT @id_pedido, id_producto, id_cliente, precio, preciototal
                FROM temp_detalle";

                    using (SqlCommand cmdDetalle = new SqlCommand(queryDetalle, connection, transaction))
                    {
                        // Añade el parámetro @id_pedido
                        cmdDetalle.Parameters.AddWithValue("@id_pedido", idPedido);

                        // Ejecuta la consulta
                        nro = cmdDetalle.ExecuteNonQuery();
                    }

                    // Si todo es correcto, confirma la transacción
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Si hay algún error, revierte la transacción
                    transaction.Rollback();
                    Console.WriteLine("Error al insertar el pedido y sus detalles: " + ex.Message);
                    return -1;
                }
            }

            return nro;
        }
    }
}