<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ABMAeropuerto.aspx.cs" Inherits="ABMAeropuerto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 93%;
            height: 281px;
        }
        .style2
        {
            width: 204px;
        }
        .style3
        {
            text-align: center;
        }
        .style4
        {
            width: 204px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="style1">
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3" colspan="2">
                <strong>Mantenimiento Aeropuerto</strong></td>
        </tr>
        <tr>
            <td class="style2">
                Codigo Aeropuerto</td>
            <td>
                <asp:TextBox ID="txtCodigoA" runat="server" Width="72px"></asp:TextBox>
            </td>
            <td style="text-align: center">
                <asp:Button ID="btnBuscarA" runat="server" onclick="btnBuscarA_Click" 
                    Text="Buscar" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                Codigo Ciudad</td>
            <td>
                <asp:TextBox ID="txtCodigoC" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblCiudad" runat="server"></asp:Label>
            </td>
            <td style="text-align: center">
                <asp:Button ID="btnBuscarC" runat="server" onclick="btnBuscarC_Click" 
                    Text="Buscar" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                Nombre Aeropuerto</td>
            <td colspan="2">
                <asp:TextBox ID="txtNombreA" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Dirección</td>
            <td colspan="2">
                <asp:TextBox ID="txtDireccion" runat="server" Width="323px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Impuesto Aeropuerto de Origen</td>
            <td colspan="2">
                <asp:TextBox ID="txtImpuestoO" runat="server" Width="81px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Impuesto Aeropuerto de Destino</td>
            <td colspan="2">
                <asp:TextBox ID="txtImpuestoD" runat="server" Width="76px"></asp:TextBox>
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
            <td class="style3">
                <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                    Text="Agregar" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnModificar" runat="server" onclick="btnModificar_Click" 
                    Text="Modificar" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEliminar" runat="server" onclick="BtnEliminar_Click" 
                    Text="Eliminar" />
            </td>
            <td class="style3">
                <asp:Button ID="Limpiar" runat="server" onclick="BtnLimpiar_Click" 
                    Text="Limpiar / Deshacer" />
            </td>
        </tr>
        <tr>
            <td class="style4">
                &nbsp;</td>
            <td class="style3" colspan="2">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

