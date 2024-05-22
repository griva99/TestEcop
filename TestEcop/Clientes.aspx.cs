using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TestEcop.Clases;

namespace TestEcop
{
    public partial class Clientes : System.Web.UI.Page
    {
        ClienteDataAccess clienteaccess;
        ClienteDDL clienteddl;
        Cliente cliente;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clienteaccess = new ClienteDataAccess();
                clienteddl = new ClienteDDL();
                bt_actualizar.Visible = false;
                lbl_mensaje_S.Visible = false;
                lbl_mensaje_E.Visible = false;
                clienteddl.DropDownList(ddl_documento);
                LoadClientes();
            }

        }
        private void LoadClientes()
        {
            List<Cliente> clientes = clienteaccess.GetClientes();
            grid_clientes.DataSource = clientes;
            grid_clientes.DataBind();
        }

        protected void bt_crear_Click(object sender, EventArgs e)
        {
            clienteddl = new ClienteDDL();
            cliente = new Cliente();
            if (Int16.TryParse(txb_codigo.Text, out short result))
            {
                cliente.codigo = result;
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: El codigo debe ser numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            if (Int64.TryParse(txb_ci.Text, out long ci))
            {
                cliente.nrodoc = ci.ToString();
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: El numero de documento debe ser numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            cliente.nombre = txb_nombre.Text;
            cliente.apellido = txb_apellido.Text;
            cliente.nrodoc = txb_ci.Text;
            cliente.id_doc = Convert.ToInt16(ddl_documento.SelectedValue);
            if (clienteddl.crear(cliente)>0)
            {
                lbl_mensaje_S.Text = "Creacion realizada correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                txb_codigo.Text = "";
                txb_nombre.Text = "";
                txb_apellido.Text = "";
                txb_ci.Text = "";
                ddl_documento.ClearSelection();
                clienteaccess = new ClienteDataAccess();
                LoadClientes();
            }
        }

        protected void grid_clientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txb_codigo.Text = grid_clientes.SelectedRow.Cells[2].Text;
            txb_nombre.Text = grid_clientes.SelectedRow.Cells[3].Text;
            txb_apellido.Text = grid_clientes.SelectedRow.Cells[4].Text;
            int indice = grid_clientes.SelectedRow.RowIndex;
            ddl_documento.SelectedValue = grid_clientes.DataKeys[indice].Values["id_doc"].ToString();
            txb_ci.Text = grid_clientes.SelectedRow.Cells[6].Text;
            bt_actualizar.Visible = true;
            lbl_mensaje_E.Visible = false;
            lbl_mensaje_S.Visible = false;
        }

        protected void grid_clientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            clienteddl = new ClienteDDL();
            cliente = new Cliente();
            cliente.id = Convert.ToInt16(e.Keys["id"]);
            if (clienteddl.EliminarCliente(cliente.id) > 0)
            {
                lbl_mensaje_S.Text = "Registro eliminado correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                clienteaccess = new ClienteDataAccess();
                LoadClientes();
            }
        }

        protected void bt_actualizar_Click(object sender, EventArgs e)
        {
            clienteddl = new ClienteDDL();
            cliente = new Cliente();
            if (Int16.TryParse(txb_codigo.Text, out short cod))
            {
                cliente.codigo = cod;
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: El codigo debe ser numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            if (Int64.TryParse(txb_ci.Text, out long ci))
            {
                cliente.nrodoc = ci.ToString();
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: El numero de documento debe ser numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            cliente.nombre = txb_nombre.Text;
            cliente.apellido = txb_apellido.Text;
            cliente.nrodoc = txb_ci.Text;
            cliente.id_doc = Convert.ToInt16(ddl_documento.SelectedValue);
            cliente.id = Convert.ToInt16(grid_clientes.SelectedValue);
            if (clienteddl.ActualizarCliente(cliente) > 0)
            {
                lbl_mensaje_S.Text = "Actualizacion realizada correctamente";
                lbl_mensaje_S.Visible = true;
                txb_codigo.Text = "";
                txb_nombre.Text = "";
                txb_apellido.Text = "";
                txb_ci.Text = "";
                ddl_documento.ClearSelection();
                clienteaccess = new ClienteDataAccess();
                LoadClientes();
            }
        }
    }
}