<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VentaPasajes.aspx.cs" Inherits="VentaPasajes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 154px;
        }
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 154px;
            text-align: center;
        }
        .style5
        {
            width: 153px;
        }
        .style7
        {
            width: 39px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="style1">
        <tr>
            <td class="style4">
                &nbsp;</td>
            <td class="style3" colspan="4">
                <strong>Venta de Pasajes</strong></td>
        </tr>
        <tr>
            <td class="style2">
                Codigo de Vuelo</td>
            <td class="style5">
                <asp:TextBox ID="txtCodigoV" runat="server"></asp:TextBox>
            </td>
            <td class="style7" colspan="2">
                <asp:Button ID="btnBuscarV" runat="server" Text="Buscar" 
                    onclick="btnBuscarV_Click" />
            </td>
            <td class="style3">
                <asp:Label ID="lblCodigoV" runat="server" ForeColor="Black" 
                    style="font-size: medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Pasaporte</td>
            <td class="style5">
                <asp:TextBox ID="txtPasaporte" runat="server"></asp:TextBox>
            </td>
            <td class="style7" colspan="2">
                <asp:Button ID="btnBuscarP" runat="server" Text="Buscar" 
                    onclick="btnBuscarP_Click" />
            </td>
            <td class="style3">
                <asp:Label ID="lblPasaporte" runat="server" ForeColor="Black" 
                    style="font-size: medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Fecha de venta</td>
            <td colspan="4">
                <asp:Label ID="lblFechaV" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Precio Total&nbsp;</td>
            <td colspan="4">
                <asp:Label ID="lblPrecioT" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Numero de venta</td>
            <td colspan="2">
                <asp:Label ID="lblNumeroV" runat="server" ForeColor="Black"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Button ID="btnAgregarP" runat="server" onclick="btnAgregarP_Click" 
                    Text="Agregar Pasaje" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnLimpiar" runat="server" onclick="BtnLimpiar_Click" 
                    Text="Limpiar / Deshacer" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td colspan="4">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

