using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TestEcop.Clases
{
    public class Producto
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public int id_und { get; set; }
        public float precio { get; set; }
        public string desunidad { get; set; }
    }
    public class ProductoDDL
    {
        ConexionDB conexionDB { get; set; } = new ConexionDB();
        protected DataTable GetTipoUnidad()
        {
            DataTable dt = new DataTable();
            string query = "SELECT id, descripcion FROM undme";
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

        public void DropDownList(DropDownList e)
        {
            e.Items.Clear();
            e.Items.Add(new ListItem("Elige un tipo de unidad", "0"));
            e.AppendDataBoundItems = true;
            e.DataSource = GetTipoUnidad();
            e.DataTextField = "descripcion";
            e.DataValueField = "id";
            e.DataBind();
        }
        public int crear(Producto producto)
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "INSERT INTO productos (codigo, descripcion, id_und, precio) VALUES (@codigo, @descripcion, @id_und, @precio)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@codigo", producto.codigo);
                        sc.Parameters.AddWithValue("@descripcion", producto.descripcion);
                        sc.Parameters.AddWithValue("@id_und", producto.id_und);
                        sc.Parameters.AddWithValue("@precio", producto.precio);
                        nro = sc.ExecuteNonQuery();
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
        public int ActualizarProducto(Producto producto)
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "UPDATE productos SET codigo = @codigo, descripcion = @descripcion, id_und = @id_und, precio = @precio WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@codigo", producto.codigo);
                        sc.Parameters.AddWithValue("@descripcion", producto.descripcion);
                        sc.Parameters.AddWithValue("@id_und", producto.id_und);
                        sc.Parameters.AddWithValue("@precio", producto.precio);
                        sc.Parameters.AddWithValue("@id", producto.id); // Asegúrate de incluir el ID del Producto a actualizar
                        nro = sc.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Muestra el mensaje de error.
                Console.WriteLine("Error al actualizar en la base de datos: " + ex.Message);
            }
            return nro;
        }
        public int EliminarProducto(int id)
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "DELETE FROM productos WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@id", id);
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
    }
    public class ProductoDataAccess
    {
        ConexionDB conexionDB { get; set; } = new ConexionDB();

        public List<Producto> GetProductos()
        {

            string connectionString = conexionDB.ConexionString();
            List<Producto> productos = new List<Producto>();

            string query = "SELECT p.id, p.codigo, p.descripcion, p.precio, p.id_und, e.descripcion AS desunidad " +
                           "FROM [productos] AS p " +
                           "JOIN [undme] AS e ON p.id_und = e.id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        id = reader.GetInt32(0),
                        codigo = reader.GetInt32(1),
                        descripcion = reader.GetString(2),
                        precio = (float)reader.GetDouble(3), // Convertir de double a float
                        id_und = reader.GetInt32(4),
                        desunidad = reader.GetString(5)
                    };

                    productos.Add(producto);
                }

                reader.Close();
            }

            return productos;
        }
    }
}