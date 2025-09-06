<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 218px;
        }
        .style3
        {
            text-align: center;
            font-size: xx-large;
        }
        .style4
        {
            width: 218px;
            text-align: center;
            font-size: x-large;
        }
        .style5
        {
            text-align: center;
        }
        .style6
        {
            width: 218px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr>
                <td class="style4">
                    <em></em>
                </td>
                <td class="style3">
                    <strong><em>Bienvenido</em></strong></td>
            </tr>
            <tr>
                <td class="style2">
                    Seleccione el Aeropuerto</td>
                <td>
                    <asp:DropDownList ID="DDLAeropuertos" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DDLAeropuertos_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Partidas</td>
                <td>
                    <asp:GridView ID="GVPartidas" runat="server">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    Llegadas</td>
                <td>
                    <asp:GridView ID="GVLlegadas" runat="server">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td class="style5">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style6">
                    &nbsp;</td>
                <td class="style5">
                    <asp:LinkButton ID="LbLogeoC" runat="server" PostBackUrl="~/LogeoCliente.aspx">Logearse comomo Cliente</asp:LinkButton>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="LbLogeoE" runat="server" PostBackUrl="~/LogeoEmpleado.aspx">Logearse como Empleado</asp:LinkButton>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
