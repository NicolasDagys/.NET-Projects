<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AltaVuelo.aspx.cs" Inherits="AltaVuelo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 141px;
        }
        .style3
        {
            width: 141px;
            font-weight: bold;
        }
        .style4
        {
            font-weight: bold;
            text-align: center;
        }
    .style5
    {
        text-align: center;
    }
    .style6
    {
        width: 141px;
        text-align: center;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="style1">
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                Alta Vuelos</td>
        </tr>
        <tr>
            <td class="style2">
                Codigo</td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Aeropuerto de Partida</td>
            <td>
                <asp:DropDownList ID="DdlAeropuertosP" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Aeropuerto de Llegada</td>
            <td>
                <asp:DropDownList ID="DdlAeropuertoL" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Fecha y Hora de partida</td>
            <td>
                <asp:TextBox ID="txtFechaP" runat="server" Width="183px"></asp:TextBox>
                &nbsp;&nbsp; (aaaa-mm-dd hh:mm)</td>
        </tr>
        <tr>
            <td class="style2">
                Fecha y Hora de Llegada</td>
            <td>
                <asp:TextBox ID="txtFechaL" runat="server" Width="178px"></asp:TextBox>
                &nbsp;&nbsp; (aaaa-mm-dd hh:mm)</td>
        </tr>
        <tr>
            <td class="style2">
                Precio del Vuelo</td>
            <td>
                <asp:TextBox ID="txtPrecioV" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Cantidad de Asientos</td>
            <td>
                <asp:TextBox ID="txtCantA" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Codigo de Vuelo</td>
            <td id="CodigoV">
                <asp:Label ID="lblCodigoV" runat="server" ForeColor="Black"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style6">
                &nbsp;</td>
            <td class="style5">
                <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                    Text="Agregar" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnLimpiar" runat="server" onclick="BtnLimpiar_Click" 
                    Text="Limpiar / Deshacer" />
&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td>
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

