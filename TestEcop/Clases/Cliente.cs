using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TestEcop.Clases
{
    public class Cliente
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int id_doc { get; set; }
        public string nrodoc { get; set; }
        public string destipo { get; set; }       
    }
    public class ClienteDDL
    {
        ConexionDB conexionDB { get; set; } = new ConexionDB();
        protected DataTable GetTipoDocumentos()
        {
            DataTable dt = new DataTable();
            string query = "SELECT id, descripcion FROM tipodoc";
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
            e.Items.Add(new ListItem("Elige un tipo de documento", "0"));
            e.AppendDataBoundItems = true;
            e.DataSource = GetTipoDocumentos();
            e.DataTextField = "descripcion";
            e.DataValueField = "id";
            e.DataBind();
        }
        public int crear(Cliente cliente)
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "INSERT INTO clientes (codigo, nombre, apellido, id_doc, nrodoc) VALUES (@codigo, @nombre, @apellido,@id_doc, @nrodoc)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@codigo", cliente.codigo);
                        sc.Parameters.AddWithValue("@nombre", cliente.nombre);
                        sc.Parameters.AddWithValue("@apellido", cliente.apellido);
                        sc.Parameters.AddWithValue("@id_doc", cliente.id_doc);
                        sc.Parameters.AddWithValue("@nrodoc", cliente.nrodoc);
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
        public int ActualizarCliente(Cliente cliente)
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "UPDATE clientes SET codigo = @codigo, nombre = @nombre, apellido = @apellido, id_doc = @id_doc, nrodoc = @nrodoc WHERE id = @id";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand sc = new SqlCommand(sqlQuery, con))
                    {
                        // Usa parámetros para evitar la inyección SQL.
                        sc.Parameters.AddWithValue("@codigo", cliente.codigo);
                        sc.Parameters.AddWithValue("@nombre", cliente.nombre);
                        sc.Parameters.AddWithValue("@apellido", cliente.apellido);
                        sc.Parameters.AddWithValue("@id_doc", cliente.id_doc);
                        sc.Parameters.AddWithValue("@nrodoc", cliente.nrodoc);
                        sc.Parameters.AddWithValue("@id", cliente.id); // Asegúrate de incluir el ID del cliente a actualizar
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
        public int EliminarCliente(int id)
        {
            int nro = 0;
            string connectionString = conexionDB.ConexionString();
            string sqlQuery = "DELETE FROM clientes WHERE id = @id";

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
    public class ClienteDataAccess
    {
        ConexionDB conexionDB { get; set; } = new ConexionDB();

        public List<Cliente> GetClientes()
        {

            string connectionString = conexionDB.ConexionString();
            List<Cliente> clientes = new List<Cliente>();

            string query = "SELECT c.id,c.codigo, c.nombre, c.apellido, c.id_doc, c.nrodoc, e.descripcion AS destipo " +
            "FROM [clientes] AS c " +
            "JOIN [tipodoc] AS e ON c.id_doc = e.id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        id = reader.GetInt32(0),
                        codigo = reader.GetInt32(1),
                        nombre = reader.GetString(2),
                        apellido = reader.GetString(3),
                        id_doc = reader.GetInt32(4),
                        nrodoc = reader.GetString(5),
                        destipo = reader.GetString(6)
                    };

                    clientes.Add(cliente);
                }

                reader.Close();
            }

            return clientes;
        }
    }
}
