<%@ Page Title="Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="TestEcop.Clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
    <h1>Formulario clientes</h1>
    <asp:Label ID="lbl_codigo" runat="server" Text="Codigo"></asp:Label>
    <asp:TextBox ID="txb_codigo" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:Label ID="lbl_nombre" runat="server" Text="Nombre"></asp:Label>
    <asp:TextBox ID="txb_nombre" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:Label ID="lbl_apellido" runat="server" Text="Apellido"></asp:Label>
    <asp:TextBox ID="txb_apellido" runat="server" CssClass="form-control"></asp:TextBox>
    <asp:Label ID="lbl_tipo" runat="server" Text="Documento"></asp:Label>
    <asp:DropDownList ID="ddl_documento" runat="server" CssClass="form-control"></asp:DropDownList>
    <asp:Label ID="lbl_ci" runat="server" Text="Nro. documento"></asp:Label>
    <asp:TextBox ID="txb_ci" runat="server" CssClass="form-control"></asp:TextBox>
    <div class="pt-2">
    <asp:Button ID="bt_crear" runat="server" Text="Crear" CssClass="btn btn-success" />
    <asp:Button ID="bt_limpiar" runat="server" Text="Limpiar" CssClass="btn btn-info"/>
    </div>
    <asp:GridView ID="grid_clientes" runat="server" AutoGenerateColumns="False" CssClass="table" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="id,id_doc">
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
        <asp:BoundField DataField="nombre" HeaderText="Nombre"></asp:BoundField>
        <asp:BoundField DataField="apellido" HeaderText="Apellido"></asp:BoundField>
        <asp:BoundField DataField="documento" HeaderText="Tipo documento"></asp:BoundField>
        <asp:BoundField DataField="nrodoc" HeaderText="Nro. documento"></asp:BoundField>
    </Columns>
    <EmptyDataTemplate>
        Sin registros
    </EmptyDataTemplate>
</asp:GridView>
    </asp:Content>
