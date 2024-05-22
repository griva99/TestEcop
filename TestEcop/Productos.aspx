<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TestEcop.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Formulario productos</h1>
<asp:Label ID="lbl_codigo" runat="server" Text="Codigo"></asp:Label>
<asp:TextBox ID="txb_codigo" runat="server" CssClass="form-control"></asp:TextBox>
<asp:Label ID="lbl_descripcion" runat="server" Text="Descripcion"></asp:Label>
<asp:TextBox ID="txb_descripcion" runat="server" CssClass="form-control"></asp:TextBox>
<asp:Label ID="lbl_und" runat="server" Text="Unidad de medida"></asp:Label>
<asp:DropDownList ID="ddl_unidad" runat="server" CssClass="form-control"></asp:DropDownList>
<asp:Label ID="lbl_precio" runat="server" Text="Precio"></asp:Label>
<asp:TextBox ID="txb_precio" runat="server" CssClass="form-control"></asp:TextBox>
<asp:Label ID="lbl_mensaje_S" runat="server" BackColor="Green" ForeColor="White"></asp:Label>
<asp:Label ID="lbl_mensaje_E" runat="server" BackColor="Red" ForeColor="White"></asp:Label>
<div class="pt-2">
<asp:Button ID="bt_crear" runat="server" Text="Crear" CssClass="btn btn-success" OnClick="bt_crear_Click" />
<asp:Button ID="bt_actualizar" runat="server" Text="Actualizar" CssClass="btn btn-info" OnClick="bt_actualizar_Click"/>
</div>
<style>
    .grid-container {
        height: 200px; /* Ajusta esta altura según sea necesario */
        overflow-y: scroll;
    }
    .table {
        width: 100%;
        border-collapse: collapse;
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

<div class="grid-container">
    <asp:GridView ID="grid_productos" runat="server" AutoGenerateColumns="False" CssClass="table" DataKeyNames="id,id_und" OnRowDeleting="grid_productos_RowDeleting" OnSelectedIndexChanged="grid_productos_SelectedIndexChanged">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="cl_select" runat="server" CausesValidation="False" CssClass="btn btn-outline-success" CommandName="Select" Text="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="cl_delete" runat="server" CausesValidation="False" CssClass="btn btn-outline-danger" CommandName="Delete" Text="Eliminar" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
            <asp:BoundField DataField="descripcion" HeaderText="descripcion"></asp:BoundField>
            <asp:BoundField DataField="desunidad" HeaderText="Tipo de medida"></asp:BoundField>
            <asp:BoundField DataField="precio" HeaderText="Precio"></asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            Sin registros
        </EmptyDataTemplate>
    </asp:GridView>
</div>
</asp:Content>
