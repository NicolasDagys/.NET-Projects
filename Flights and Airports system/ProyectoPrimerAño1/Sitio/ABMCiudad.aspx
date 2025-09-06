<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ABMCiudad.aspx.cs" Inherits="ABMCiudad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 89%;
            height: 211px;
        }
        .style2
        {
            width: 149px;
        }
        .style3
        {
            width: 149px;
            height: 23px;
            text-align: center;
        }
        .style4
        {
            height: 23px;
        }
        .style5
        {
            width: 149px;
            font-weight: bold;
            text-align: center;
        }
        .style6
        {
            font-weight: bold;
            text-align: center;
        }
        .style7
        {
            width: 532px;
            text-align: center;
        }
        .style8
        {
            width: 149px;
            text-align: center;
        }
        .style9
        {
            text-align: center;
        }
        .style11
        {
            width: 194px;
            height: 23px;
            text-align: center;
            font-size: large;
        }
        .style12
        {
            width: 149px;
            text-align: center;
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="style1">
        <tr>
            <td class="style5">
                &nbsp;</td>
            <td class="style6" colspan="2">
                Mantenimiento Ciudad</td>
        </tr>
        <tr>
            <td class="style11">
                Codigo
            </td>
            <td class="style4">
                <asp:TextBox ID="txtCodigo" runat="server" Width="102px"></asp:TextBox>
            </td>
            <td class="style4">
                <asp:Button ID="btnBuscar" runat="server" onclick="btnBuscar_Click" 
                    Text="Buscar" />
            </td>
        </tr>
        <tr>
            <td class="style11">
                Nombre</td>
            <td class="style4" colspan="2">
                <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style12">
                Pais</td>
            <td colspan="2">
                <asp:TextBox ID="txtPais" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style8">
                &nbsp;</td>
            <td class="style7">
                <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                    Text="Agregar" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnModificar" runat="server" onclick="btnModificar_Click" 
                    Text="Modificar" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEliminar" runat="server" onclick="btnEliminar_Click" 
                    Text="Eliminar" />
            </td>
            <td class="style9">
                <asp:Button ID="btnLimpiar" runat="server" onclick="btnLimpiar_Click" 
                    Text="Limpiar / Deshacer" />
            </td>
        </tr>
        <tr>
            <td class="style8">
                &nbsp;</td>
            <td class="style9" colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

