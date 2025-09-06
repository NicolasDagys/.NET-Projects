<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ABMCliente.aspx.cs" Inherits="ABMCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 186px;
        }
        .style3
        {
            width: 186px;
            height: 20px;
            text-align: center;
        }
        .style4
        {
            height: 20px;
            font-weight: bold;
            text-align: center;
        }
        .style5
        {
            text-align: center;
        }
        .style6
        {
            width: 186px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="style1">
        <tr>
            <td class="style3">
            </td>
            <td class="style4" colspan="2">
                Mantenimiento Clientes</td>
        </tr>
        <tr>
            <td class="style2">
                Pasaporte</td>
            <td>
                <asp:TextBox ID="txtPasaporte" runat="server" Width="176px"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                    onclick="btnBuscar_Click" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                Nombre</td>
            <td colspan="2">
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Contraseña</td>
            <td colspan="2">
                <asp:TextBox ID="txtContrasenia" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Numero de Tarjeta</td>
            <td colspan="2">
                <asp:TextBox ID="txtNumTarjeta" runat="server" Width="236px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" 
                    onclick="btnAgregar_Click" />
&nbsp;&nbsp;
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" 
                    onclick="btnModificar_Click" />
&nbsp;&nbsp;
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                    onclick="btnEliminar_Click" />
            </td>
            <td>
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar / Deshacer" 
                    onclick="BtnLimpiar_Click" />
            </td>
        </tr>
        <tr>
            <td class="style6">
                &nbsp;</td>
            <td class="style5" colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

