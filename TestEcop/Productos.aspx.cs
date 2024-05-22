using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Microsoft.SqlServer;
using TestEcop.Clases;
using WebGrease.Activities;

namespace TestEcop
{
    public partial class Productos : System.Web.UI.Page
    {
        ProductoDataAccess productoaccess;
        ProductoDDL productoddl;
        Producto producto;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                productoaccess = new ProductoDataAccess();
                productoddl = new ProductoDDL();
                bt_actualizar.Visible = false;
                lbl_mensaje_S.Visible = false;
                lbl_mensaje_E.Visible = false;
                productoddl.DropDownList(ddl_unidad);
                LoadProductos();
            }

        }
        private void LoadProductos()
        {
            List<Producto> productos = productoaccess.GetProductos();
            grid_productos.DataSource = productos;
            grid_productos.DataBind();
        }

        protected void bt_crear_Click(object sender, EventArgs e)
        {
            productoddl = new ProductoDDL();
            producto = new Producto();
            if (Int32.TryParse(txb_codigo.Text, out int cod))
            {
                producto.codigo = cod;
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: El codigo debe ser numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            if (Int32.TryParse(txb_precio.Text, out int precio))
            {
                producto.precio = precio;
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: Precio no cumple con formato numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            producto.descripcion = txb_descripcion.Text;
            producto.id_und = Convert.ToInt16(ddl_unidad.SelectedValue);
            if (productoddl.crear(producto) > 0)
            {
                lbl_mensaje_S.Text = "Creacion realizada correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                txb_codigo.Text = "";
                txb_descripcion.Text = "";
                txb_precio.Text = "";
                ddl_unidad.ClearSelection();
                productoaccess = new ProductoDataAccess();
                LoadProductos();
            }
        }

        protected void grid_productos_SelectedIndexChanged(object sender, EventArgs e)
        {
            txb_codigo.Text = grid_productos.SelectedRow.Cells[2].Text;
            txb_descripcion.Text = grid_productos.SelectedRow.Cells[3].Text;
            txb_precio.Text = grid_productos.SelectedRow.Cells[5].Text;
            int indice = grid_productos.SelectedRow.RowIndex;
            ddl_unidad.SelectedValue = grid_productos.DataKeys[indice].Values["id_und"].ToString();
            bt_actualizar.Visible = true;
            lbl_mensaje_E.Visible = false;
            lbl_mensaje_S.Visible = false;
        }

        protected void grid_productos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            productoddl = new ProductoDDL();
            producto = new Producto();
            producto.id = Convert.ToInt16(e.Keys["id"]);
            if (productoddl.EliminarProducto(producto.id) > 0)
            {
                lbl_mensaje_S.Text = "Registro eliminado correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                productoaccess = new ProductoDataAccess();
                LoadProductos();
            }
        }

        protected void bt_actualizar_Click(object sender, EventArgs e)
        {
            productoddl = new ProductoDDL();
            producto = new Producto();
            if (Int16.TryParse(txb_codigo.Text, out short cod))
            {
                producto.codigo = cod;
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: El codigo debe ser numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            if (Int32.TryParse(txb_precio.Text, out int precio))
            {
                producto.precio = precio;
                // Aquí puedes agregar cualquier lógica adicional que necesites con el valor convertido.
            }
            else
            {
                // Muestra un mensaje de error si la conversión falla.
                lbl_mensaje_E.Text = "Error: Precio no cumple con formato numerico.";
                lbl_mensaje_E.Visible = true;
                return; // Sale del método si la conversión falla.
            }
            producto.descripcion = txb_descripcion.Text;
            producto.id_und = Convert.ToInt16(ddl_unidad.SelectedValue);
            producto.id = Convert.ToInt16(grid_productos.SelectedValue);
            if (productoddl.ActualizarProducto(producto) > 0)
            {
                lbl_mensaje_S.Text = "Actualizacion realizada correctamente";
                lbl_mensaje_S.Visible = true;
                txb_codigo.Text = "";
                txb_descripcion.Text = "";
                txb_precio.Text = "";
                ddl_unidad.ClearSelection();
                productoaccess = new ProductoDataAccess();
                LoadProductos();
            }
        }
    }
}