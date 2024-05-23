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
        VistaDetalle vista;
        float total;
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
                EliminarTemp();
            }
        }
        protected void grid_pedido_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            pedidovista = new PedidoVista();
            vista = new VistaDetalle();
            vista.id = Convert.ToInt16(e.Keys["id"]);
            if (pedidovista.Eliminar(vista.id) > 0)
            {
                lbl_mensaje_S.Text = "Registro eliminado correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                total = pedidovista.GetTotal();
                lbl_precio.Text = total.ToString();
                pedidovista = new PedidoVista();
                LoadGrid();
            }
        }
        private void EliminarTemp()
        {
            pedidovista = new PedidoVista();
            pedidovista.EliminarTemp();
            LoadGrid();
        }
        private void LoadGrid()
        {
            List<VistaDetalle> grilla = pedidovista.GetGrilla();
            grid_pedido.DataSource = grilla;
            grid_pedido.DataBind();
            productosEnGrilla = grilla;
        }
        protected void bt_add_Click(object sender, EventArgs e)
        {
            pedidovista = new PedidoVista();
            vista = new VistaDetalle();
            vista.id_producto = Convert.ToInt16(ddl_producto.SelectedValue);
            vista.id_cliente = Convert.ToInt16(ddl_cliente.SelectedValue);
            VistaDetalle vistatemp = pedidovista.GetVista(vista.id_cliente, vista.id_producto);
            vista.descripcion = vistatemp.descripcion;
            vista.precio = vistatemp.precio;
            vista.nombre = vistatemp.nombre;
            if (pedidovista.detalleTemp(vista) > 0)
            {
                lbl_mensaje_S.Text = "Detalle añadido correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                ddl_cliente.Enabled = false;
                ddl_producto.ClearSelection();
                total = pedidovista.GetTotal();
                lbl_precio.Text = total.ToString();
                lbl_precio.Visible = true;
                LoadGrid();
            }
        }
        protected void bt_crear_Click(object sender, EventArgs e)
        {
            pedidovista = new PedidoVista();
            pedido = new Pedido();
            detalle = new DetallePedido();
            total = pedidovista.GetTotal();
            if (pedidovista.guardar(total,grid_pedido.Rows.Count) > 0)
            {
                lbl_mensaje_S.Text = "Guardado correctamente";
                lbl_mensaje_S.Visible = true;
                lbl_mensaje_E.Visible = false;
                ddl_cliente.Enabled = true;
                ddl_cliente.ClearSelection();
                ddl_producto.ClearSelection();
                grid_pedido.DataSource = null;
                grid_pedido.DataBind();
                total = 0;
                lbl_precio.Visible = false;
                EliminarTemp();
            }
            else
            {
                lbl_mensaje_E.Text = "Error de carga";
                lbl_mensaje_S.Visible = false;
                lbl_mensaje_E.Visible = true;
            }
        }

    }
}