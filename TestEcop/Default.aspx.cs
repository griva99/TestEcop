using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TestEcop.Clases;

namespace TestEcop
{
    public partial class _Default : Page
    {
        Pedido pedido;
        DetallePedido detalle;
        PedidoDDL pedidoDDL;
        PedidoVista pedidovista;
        float total = 0;
        private List<VistaDetalle> productosEnGrilla = new List<VistaDetalle>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pedidoDDL = new PedidoDDL();
                lbl_mensaje_S.Visible = false;
                lbl_mensaje_E.Visible = false;
                pedidoDDL.DropDownListProducto(ddl_producto);
                pedidoDDL.DropDownListCliente(ddl_cliente);
                grid_pedido.DataSource = productosEnGrilla;
                grid_pedido.DataBind();
            }
        }
        protected void grid_pedido_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Cancelar la eliminación para evitar que se elimine la fila de la base de datos
            e.Cancel = true;

            // Ocultar la fila que se está eliminando
            grid_pedido.Rows[e.RowIndex].Visible = false;
        }
        protected void bt_add_Click(object sender, EventArgs e)
        {
            pedidovista = new PedidoVista();
            // Obtiene los nuevos productos y los agrega a la lista existente de productos en la grilla
            List<VistaDetalle> nuevosProductos = pedidovista.GetVista(Convert.ToInt16(ddl_cliente.SelectedValue), Convert.ToInt16(ddl_producto.SelectedValue));
            productosEnGrilla.AddRange(nuevosProductos);
            grid_pedido.DataSource = productosEnGrilla;
            grid_pedido.DataBind();

            // Calcula el precio total de todos los productos en la grilla
            
            foreach (var producto in nuevosProductos)
            {
                total += producto.precio;
            }
            lbl_precio.Text = total.ToString();
        }
        protected void bt_crear_Click(object sender, EventArgs e)
        {
            pedidovista = new PedidoVista();
            pedido = new Pedido();
            detalle = new DetallePedido();
            pedido.preciototal = productosEnGrilla.Sum(p => p.precio);
            pedido.cantidad = productosEnGrilla.Count;
            pedido.detalle = productosEnGrilla.Select(p => new DetallePedido
            {
                id_cliente = Convert.ToInt16(ddl_cliente.SelectedValue),
                id_producto = p.id_producto,
                precio = p.precio,
                preciototal = total 
            }).ToList();
            if (pedidovista.guardar(pedido) > 0)
            {
                lbl_mensaje_S.Text = "Guardado correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                ddl_cliente.ClearSelection();
                ddl_producto.ClearSelection();
                grid_pedido.DataSource = null;
                grid_pedido.DataBind();

            }
        }

    }
}