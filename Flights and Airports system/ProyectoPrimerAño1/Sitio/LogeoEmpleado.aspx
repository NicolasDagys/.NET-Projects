<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogeoEmpleado.aspx.cs" Inherits="LogeoEmpleado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style6
        {
            width: 41%;
            height: 111px;
        }
        .style7
        {
            width: 182px;
        }
        .style8
        {
            text-align: right;
        }
        .style9
        {
            width: 182px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table class="style6">
        <tr>
            <td class="style7">
                Usuario</td>
            <td>
                <asp:TextBox ID="TxtUsuario" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style7">
                Contraseña</td>
            <td>
                <asp:TextBox ID="TxtContra" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                &nbsp;</td>
            <td class="style8">
                <asp:Button ID="BtnLogeo" runat="server" onclick="BtnLogeo_Click" 
                    Text="Login" />
            </td>
        </tr>
        <tr>
            <td class="style7">
                &nbsp;</td>
            <td>
                <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
