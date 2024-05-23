<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestEcop._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Carga de pedidos</h1>
            <asp:Label ID="lbl_cliente" runat="server" Text="Cliente"></asp:Label>
            <asp:DropDownList ID="ddl_cliente" runat="server" CssClass="form-control"></asp:DropDownList>
            <asp:Label ID="lbl_producto" runat="server" Text="Producto"></asp:Label>
            <asp:DropDownList ID="ddl_producto" runat="server" CssClass="form-control"></asp:DropDownList>
            <asp:Button ID="bt_add" runat="server" Text="Agregar" CssClass="btn btn-info" OnClick="bt_add_Click"/>
                    <asp:GridView ID="grid_pedido" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="id,id_producto" OnRowDeleting="grid_pedido_RowDeleting">
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:Button ID="cl_delete" runat="server" CausesValidation="False" CssClass="btn btn-outline-danger" CommandName="Delete" Text="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="nombre" HeaderText="Nombre"></asp:BoundField>
                            <asp:BoundField DataField="descripcion" HeaderText="Producto" />
                            <asp:BoundField DataField="precio" HeaderText="Precio"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            Sin registros
                        </EmptyDataTemplate>
                    </asp:GridView>
        </section>

        <div class="row">            
    <asp:Label ID="lbl_mensaje_S" runat="server" BackColor="Green" ForeColor="White"></asp:Label>
    <asp:Label ID="lbl_mensaje_E" runat="server" BackColor="Red" ForeColor="White"></asp:Label>
            <section class="col-md-4" aria-labelledby="gettingStartedTitle">
                <div class="grid-container">
                <h2>Precio Total</h2>
                <asp:Label ID="lbl_precio" runat="server"></asp:Label>
                </div>
            </section>
            <section class="col-md-4" aria-labelledby="librariesTitle">
                <asp:Button ID="bt_guardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="bt_crear_Click"/>
            </section>
        </div>
    </main>
    <style>
    .grid-container {
        height: 200px; /* Ajusta esta altura según sea necesario */
        overflow-y: scroll;
    }
    .table {
        width: 100%;
        border-collapse: collapse;
            margin-top: 96px;
        }
    .table th, .table td {
        border: 1px solid #ddd;
        padding: 8px;
    }
    .table th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: left;
        background-color: #f2f2f2;
    }
</style>
</asp:Content>
