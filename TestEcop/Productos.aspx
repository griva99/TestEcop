<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="TestEcop.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Formulario productos</h1>
    <asp:Label ID="lbl_codigo" runat="server" Text="Codigo"></asp:Label>
    <asp:TextBox ID="txb_codigo" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:Label ID="lbl_descripcion" runat="server" Text="Descripcion"></asp:Label>
    <asp:TextBox ID="txb_descripcion" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:Label ID="lbl_medida" runat="server" Text="Unidad de medida"></asp:Label>
    <asp:DropDownList ID="ddl_medida" runat="server" CssClass="form-control"></asp:DropDownList>
    <asp:Label ID="lbl_precio" runat="server" Text="Precio unitario"></asp:Label>
    <asp:TextBox ID="txb_precio" runat="server" CssClass="form-control"></asp:TextBox>
    <div class="pt-2">
        <asp:Button ID="bt_crear" runat="server" Text="Crear" CssClass="btn btn-success" />
        <asp:Button ID="bt_limpiar" runat="server" Text="Limpiar" CssClass="btn btn-info" />
    </div>

    <asp:GridView ID="grid_productos" runat="server" AutoGenerateColumns="False" CssClass="table" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="id,id_und">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="pr_select" runat="server" CausesValidation="False" CssClass="btn btn-outline-success" CommandName="Select" Text="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="pr_delete" runat="server" CausesValidation="False" CssClass="btn btn-outline-danger" CommandName="Delete" Text="Eliminar" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="codigo" HeaderText="Codigo" />
            <asp:BoundField DataField="descripcion" HeaderText="Descripcion"></asp:BoundField>
            <asp:BoundField DataField="unidad" HeaderText="Unidad de medida"></asp:BoundField>
            <asp:BoundField DataField="precio" HeaderText="Precio unitario"></asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            Sin registros
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
